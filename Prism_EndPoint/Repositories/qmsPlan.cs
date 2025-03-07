using Microsoft.EntityFrameworkCore;
using Prism_EndPoint.Entities;
using Prism_EndPoint.Models;
using System.Diagnostics;
using System.Numerics;

namespace Prism_EndPoint.Repositories
{

    public class qmsPlan : IqmsPlan
    {
        private readonly PrismDbContext _QmsContext;
        public qmsPlan(PrismDbContext QmsContext)
        {
            _QmsContext = QmsContext;
        }
        public async Task<IEnumerable<PlanDivisionTeam>> getDivisionPlanTeam(int role, int code, string team)
        {
            List<FrequencyAudit> divisionProcessPlan = null;
            if (role == 1 || role == 2)
            {
                divisionProcessPlan = await _QmsContext.FrequencyAudits
                   .Include(x => x.AuditTeamNavigation)
                       .ThenInclude(x => x.QmsteamMembers)
                   .Where(x => x.ProgramId == code)
            .ToListAsync();
            }
            else
            {
                divisionProcessPlan = await _QmsContext.FrequencyAudits
                   .Include(x => x.AuditTeamNavigation)
                       .ThenInclude(x => x.QmsteamMembers)
                   .Where(x =>
                       x.AuditTeamNavigation != null &&
                       (x.AuditTeamNavigation.TeamLeader == team ||
                        x.AuditTeamNavigation.QmsteamMembers.Any(m => m.Member == team)))
                   .Where(x => x.ProgramId == code)
            .ToListAsync();
            }

            List<PlanDivisionTeam> DivisionPlanTeam = divisionProcessPlan.Select(e => new PlanDivisionTeam
            {
                Id = e.DivisionId,

            }).ToList();

            return DivisionPlanTeam;
        }


        public async Task<IEnumerable<planDivisionTable>> getDivisionprocessPlan(string divisionId, int code)
        {
            var Process = await _QmsContext.Qmsprocesses
                            .Include(x => x.DivisionProcesses)
                            .Include(x => x.QmssubProcesses)
                            .Where(x => x.DivisionProcesses.Any(z => z.DivisionId == divisionId))
                            .ToListAsync();
            int divisionIdInt = Convert.ToInt32(divisionId);
            List<planDivisionTable> processTeam = Process.Select(e => new planDivisionTable
            {
                Id = e.Id,
                Process = e.ProcessTitle,
                Owner = e.DivisionProcesses.FirstOrDefault().ProcessOwnerId,
                Status = _QmsContext.QmsPlans.Where(z => z.DivisionId == divisionIdInt).Where(x=>x.ProcessId == e.Id).Where(x => x.ProgramId == code).FirstOrDefault()?.Status,
            }).ToList();



            return processTeam;
        }

        public async Task<qmsPlanEntities> getProcessPlan(int code, int divId, int progId)
        {
            qmsPlanEntities plan = null;


            var processPlan = await _QmsContext.Qmsprocesses
                .Where(x => x.QmsPlans.Any(p => p.ProcessId == code && p.ProgramId == progId))
                .Include(x => x.DivisionProcesses)
                .Include(x => x.QmssubProcesses)
                .Include(x => x.QmsPlans
                    .Where(p => p.ProcessId == code && p.ProgramId == progId)
                    .OrderBy(p => p.Id)
                    .Take(1)
                )
                .ThenInclude(y => y.QmsPlanAudits)
                .ThenInclude(z => z.QmsPlanAuditClauses)
                .FirstOrDefaultAsync();


            var teamlead = await _QmsContext.FrequencyAudits
                                            .Include(x => x.AuditTeamNavigation)
                                            .Where(x => x.DivisionId == divId)
                                            .Where(x => x.ProgramId == progId)
                                            .FirstOrDefaultAsync();

            if (processPlan == null)
            {
                var subProcesses = await _QmsContext.Qmsprocesses
                    .Include(x => x.QmssubProcesses)
                    .Include(x => x.DivisionProcesses)
                    .Where(x => x.DivisionProcesses.Any(dp => dp.DivisionId == divId.ToString()))
                    .SelectMany(audits => audits.QmssubProcesses.Select(subprocess => new
                    {
                        SubProcessId = subprocess.SubProcessId,
                        SubProcessTitle = subprocess.SubProcessName,
                        ProcessOwner = audits.DivisionProcesses
                            .Select(dp => dp.ProcessOwnerId)
                            .FirstOrDefault()
                    }))
                    .ToListAsync();

                var planNull = new qmsPlanEntities
                {
                    ProcessTitle = await _QmsContext.Qmsprocesses
                                                     .Where(x => x.Id == code)
                                                     .Select(x => x.ProcessTitle)
                                                     .FirstOrDefaultAsync(),
                    Id = code,
                    divId = divId,
                    teamLead = teamlead?.AuditTeamNavigation?.TeamLeader,
                    membersTeams = (await _QmsContext.FrequencyAudits
                                                      .Include(x => x.AuditTeamNavigation)
                                                      .ThenInclude(z => z.QmsteamMembers)
                                                      .Where(x => x.DivisionId == divId)
                                                      .Where(x => x.ProgramId == progId)
                                                      .FirstOrDefaultAsync())?.AuditTeamNavigation?.QmsteamMembers?
                                                      .Select(members => new qmsPlanEntities.membersTeam
                                                      {
                                                          memberId = members.Member,
                                                      }).ToList() ?? new List<qmsPlanEntities.membersTeam>(),
                    PlanAudits = subProcesses.Select(subprocess => new qmsPlanEntities.PlanAudit
                    {
                        SubProcessId = subprocess.SubProcessId,
                        SubProcessTitle = subprocess.SubProcessTitle,
                        ProcessOwner = subprocess.ProcessOwner
                    }).ToList(),
                };

                return planNull;
            }

            var subProcessesWithDetails = await _QmsContext.Qmsprocesses
                .Include(x => x.QmssubProcesses)
                .Include(x => x.DivisionProcesses)
                .Where(x => x.DivisionProcesses.Any(dp => dp.DivisionId == divId.ToString()))
                .Where(x => x.Id == code)
                .SelectMany(audits => audits.QmssubProcesses.Select(subprocess => new
                {
                    SubProcessId = subprocess.SubProcessId,
                    SubProcessTitle = subprocess.SubProcessName,
                    ProcessOwner = audits.DivisionProcesses
                        .Select(dp => dp.ProcessOwnerId)
                        .FirstOrDefault()
                }))
                .ToListAsync();

            var planAudits = processPlan.QmsPlans.FirstOrDefault()?.QmsPlanAudits;

            var auditStatus = _QmsContext.QmsCheckLists
                                   .Include(x => x.QmsAuditReports)
                               .Where(x => x.AuditId == processPlan.QmsPlans.FirstOrDefault().Id)
                                .FirstOrDefault()?.Id;

            var Stats = "";
            if (auditStatus == null)
            {
                Stats = "Pending";
            }
            else
            {
                Stats = "Audited";
            }

            plan = new qmsPlanEntities
            {
                Id = code,
                ProcessTitle = processPlan.ProcessTitle,
                checklist = _QmsContext.QmsCheckLists.Where(x => x.AuditId == processPlan.QmsPlans.FirstOrDefault().Id).FirstOrDefault()?.Id,
                PlanId = processPlan.QmsPlans.FirstOrDefault()?.Id,
                AuditObj = processPlan.QmsPlans.FirstOrDefault()?.AuditObj,
                AuditMetho = processPlan.QmsPlans.FirstOrDefault()?.AuditMemo,
                teamLead = teamlead?.AuditTeamNavigation?.TeamLeader,
                divId = divId,
                AuditStatus = Stats,
                Created = processPlan.QmsPlans.FirstOrDefault()?.CreateAt,
                approveTL = processPlan.QmsPlans.FirstOrDefault()?.ApprovedDateTl,
                approveIQA = processPlan.QmsPlans.FirstOrDefault()?.ApprovedDateIqa,
                approveQMS = processPlan.QmsPlans.FirstOrDefault()?.ApprovedDateQms,
                note = processPlan.QmsPlans.FirstOrDefault()?.Notes,
                noteBy = processPlan.QmsPlans.FirstOrDefault()?.NotesBy,
                Approve = processPlan.QmsPlans.FirstOrDefault()?.Approve,

                membersTeams = (await _QmsContext.FrequencyAudits
                                                  .Include(x => x.AuditTeamNavigation)
                                                  .ThenInclude(z => z.QmsteamMembers)
                                                  .Where(x => x.DivisionId == divId)
                                                  .Where(x => x.ProgramId == progId)
                                                  .FirstOrDefaultAsync())?.AuditTeamNavigation?.QmsteamMembers?
                                                  .Select(members => new qmsPlanEntities.membersTeam
                                                  {
                                                      memberId = members.Member,
                                                  }).ToList() ?? new List<qmsPlanEntities.membersTeam>(),
                PlanAudits = subProcessesWithDetails.Select(subprocess => new qmsPlanEntities.PlanAudit
                {
                    SubProcessId = subprocess.SubProcessId,
                    SubProcessTitle = subprocess.SubProcessTitle,
                    ProcessOwner = subprocess.ProcessOwner,
                    AuditCriteria = planAudits?.FirstOrDefault(x => x.SubProcessId == subprocess.SubProcessId)?.AuditCriteria,
                    AuditDate = planAudits?.FirstOrDefault(x => x.SubProcessId == subprocess.SubProcessId)?.AuditDate,
                    TimeFrom = planAudits?.FirstOrDefault(x => x.SubProcessId == subprocess.SubProcessId)?.TimeFrom,
                    TimeTo = planAudits?.FirstOrDefault(x => x.SubProcessId == subprocess.SubProcessId)?.TimeTo,
                    auditClause = planAudits?.FirstOrDefault(x => x.SubProcessId == subprocess.SubProcessId)?.QmsPlanAuditClauses?
                    .Select(clause => new qmsPlanEntities.PlanAudit.auditClauses
                    {
                        subSclauses = clause.SubClause
                    }).ToList() ?? new List<qmsPlanEntities.PlanAudit.auditClauses>(),
                }).ToList(),
                Status = processPlan.QmsPlans.FirstOrDefault()?.Status,

            };

            return plan;
        }



        public async Task addQmsPlan(int progId, int divId, int processId)
        {
            var frequency = await _QmsContext.FrequencyAudits.Where(x => x.ProgramId == progId).Where(x => x.DivisionId == divId).FirstOrDefaultAsync();



            var newPlan = new QmsPlan
            {
                ProgramId = progId,
                DivisionId = divId,
                Status = "Drafted",
                Approve = "Pending",
                ProcessId = processId,
                FrequencyId = frequency.Id
            };
            await _QmsContext.QmsPlans.AddAsync(newPlan);
            await _QmsContext.SaveChangesAsync();



        }

        public async Task UpdatePlanObj(int planId, string objectives)
        {
            var plan = await _QmsContext.QmsPlans.Where(x => x.Id == planId).FirstOrDefaultAsync();

            if (plan != null)
            {
                try
                {
                    plan.AuditObj = objectives;
                    _QmsContext.QmsPlans.Update(plan);
                    await _QmsContext.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException("Error updating employee in the database", ex);
                }
            }
        }
        public async Task updateMetho(int planId, string objectives)
        {
            var plan = await _QmsContext.QmsPlans.Where(x => x.Id == planId).FirstOrDefaultAsync();

            if (plan != null)
            {
                try
                {
                    plan.AuditMemo = objectives;
                    _QmsContext.QmsPlans.Update(plan);
                    await _QmsContext.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException("Error updating employee in the database", ex);
                }
            }
        }

        public async Task updatePlanAudit(AuditEntry entry)
        {
            var auditPlan = await _QmsContext.QmsPlanAudits.Where(x => x.PlanId == entry.planId).Where(x => x.SubProcessId == entry.subprocId).FirstOrDefaultAsync();

            if (auditPlan == null)
            {
                try
                {
                    var process = _QmsContext.QmssubProcesses
                                    .Include(x => x.Process)
                                        .ThenInclude(y => y.DivisionProcesses)
                                        .Where(x => x.SubProcessId == entry.subprocId)
                                    .FirstOrDefault();
                    var plan = _QmsContext.QmsPlans.Include(x => x.Frequency).Where(x => x.Id == entry.planId).FirstOrDefault();
                    var newPlanAudit = new QmsPlanAudit
                    {
                        PlanId = entry.planId,
                        SubProcessId = entry.subprocId,
                        AuditCriteria = entry.criteria,
                        ProcessOwner = process.Process.DivisionProcesses.FirstOrDefault().ProcessOwnerId,
                        TeamId = plan.Frequency.AuditTeam,
                        DivisionId = plan.Frequency.DivisionId,
                        AuditDate = entry.auditDate,
                        TimeFrom = entry.from,
                        TimeTo = entry.to

                    };
                    _QmsContext.QmsPlanAudits.Add(newPlanAudit);
                    await _QmsContext.SaveChangesAsync();


                    foreach (var clause in entry.auditClause)
                    {
                        var newAuditClause = new QmsPlanAuditClause
                        {
                            SubClause = clause.subSclauses,
                            PlanAuditId = newPlanAudit.Id,
                        };
                        _QmsContext.QmsPlanAuditClauses.Add(newAuditClause);
                        await _QmsContext.SaveChangesAsync();
                    }


                    return;
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException("Error updating employee in the database", ex);
                }
            }
            auditPlan.AuditCriteria = entry.criteria;
            auditPlan.AuditDate = entry.auditDate;
            auditPlan.TimeFrom = entry.from;
            auditPlan.TimeTo = entry.to;
            _QmsContext.QmsPlanAudits.Update(auditPlan);
            await _QmsContext.SaveChangesAsync();
            var AuditClause = await _QmsContext.QmsPlanAuditClauses.Where(x => x.PlanAuditId == auditPlan.Id).ToListAsync();
            if (AuditClause.Any())
            {
                _QmsContext.QmsPlanAuditClauses.RemoveRange(AuditClause);
                await _QmsContext.SaveChangesAsync();
            }
            foreach (var clause in entry.auditClause)
            {
                var newAuditClause = new QmsPlanAuditClause
                {
                    SubClause = clause.subSclauses,
                    PlanAuditId = auditPlan.Id,
                };
                _QmsContext.QmsPlanAuditClauses.Add(newAuditClause);
                await _QmsContext.SaveChangesAsync();
            }


            return;

        }

        public async Task UpdateTeamLead(int planId)
        {
            var plan = await _QmsContext.QmsPlans.Where(x => x.Id == planId).FirstOrDefaultAsync();

            if (plan != null)
            {
                string approved;
                string status;
                if (plan.Approve == "Pending")
                {
                    approved = "TeamLeadApproved";
                    plan.Notes = null;
                    plan.NotesBy = null;
                    plan.ApprovedDateTl = DateOnly.FromDateTime(DateTime.Now);
                    status = "Draft";
                }
                else if (plan.Approve == "TeamLeadApproved")
                {
                    approved = "IQAheadApprove";
                    plan.ApprovedDateIqa = DateOnly.FromDateTime(DateTime.Now);
                    status = "Pending Review";
                }
                else if (plan.Approve == "IQAheadApprove")
                {
                    plan.ApprovedDateQms = DateOnly.FromDateTime(DateTime.Now);
                    approved = "QMSheadApprove";
                    status = "Approved";
                }
                else
                {
                    approved = "Pending";
                    status = "Draft";
                }
                try
                {
                    plan.Approve = approved;
                    
                    plan.Status = status;
                    _QmsContext.QmsPlans.Update(plan);
                    await _QmsContext.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException("Error updating employee in the database", ex);
                }
            }
        }

        public async Task returnPlan(int planId, string Conclusion)
        {

            var plan = await _QmsContext.QmsPlans.Where(x => x.Id == planId).FirstOrDefaultAsync();

            string notesBy;
            if (plan.Approve == "TeamLeadApproved")
            {

                notesBy = "from IQA Head";
            }
            else {
                notesBy = "from QMS Head";
            }
            try
            {
                plan.Approve = "Pending";
                plan.Status = "Draft";
               plan.NotesBy = notesBy;
                plan.Notes = Conclusion;
                _QmsContext.QmsPlans.Update(plan);
                await _QmsContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error updating employee in the database", ex);
            }



        }




    }
}
