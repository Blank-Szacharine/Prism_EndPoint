using Microsoft.EntityFrameworkCore;
using Prism_EndPoint.Entities;
using Prism_EndPoint.Models;
using System.Diagnostics;

namespace Prism_EndPoint.Repositories
{

    public class qmsPlan : IqmsPlan
    {
        private readonly PrismDbContext _QmsContext;
        public qmsPlan(PrismDbContext QmsContext)
        {
            _QmsContext = QmsContext;
        }
        public async Task<IEnumerable<PlanDivisionTeam>> getDivisionPlanTeam(int code, string team)
        {
            var divisionProcessPlan = await _QmsContext.FrequencyAudits
                   .Include(x => x.AuditTeamNavigation)
                       .ThenInclude(x => x.QmsteamMembers)
                   .Where(x =>
                       x.AuditTeamNavigation != null &&
                       (x.AuditTeamNavigation.TeamLeader == team ||
                        x.AuditTeamNavigation.QmsteamMembers.Any(m => m.Member == team)))
                   .Where(x => x.ProgramId == code)
            .ToListAsync();

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

            List<planDivisionTable> processTeam = Process.Select(e => new planDivisionTable
            {
                Id = e.Id,
                Process = e.ProcessTitle,
                Owner = e.DivisionProcesses.FirstOrDefault().ProcessOwnerId,
                Status = _QmsContext.QmsPlans.Include(x => x.Process).Where(z => z.Process.Id == e.Id).Where(x => x.ProcessId == code).FirstOrDefault()?.Status,
            }).ToList();

            return processTeam;
        }

        public async Task<qmsPlanEntities> getProcessPlan(int code, int divId, int progId)
        {
            qmsPlanEntities plan = null;

           
            var processPlan = await _QmsContext.Qmsprocesses
                                               .Include(x => x.DivisionProcesses)
                                               .Include(x => x.QmssubProcesses)
                                               .Include(x => x.QmsPlans)
                                               .ThenInclude(y => y.QmsPlanAudits)
                                               .Where(x => x.QmsPlans.Any(p => p.ProcessId == code))
                                               .Where(x=>x.QmsPlans.Any(p=>p.ProgramId == progId))
                                               .FirstOrDefaultAsync();

            if (processPlan == null)
            {
             
                var teamlead = await _QmsContext.FrequencyAudits
                                                .Include(x => x.AuditTeamNavigation)
                                                .Where(x => x.DivisionId == divId)
                                                .Where(x=>x.ProgramId == progId)
                                                .FirstOrDefaultAsync();

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
                    PlanAudits = await _QmsContext.Qmsprocesses
                                        .Include(x => x.QmssubProcesses)
                                        .Include(x => x.DivisionProcesses)
                                        .Where(x => x.DivisionProcesses.Any(x => x.DivisionId == divId.ToString()))
                                        .SelectMany(audits => audits.QmssubProcesses.Select(subprocess => new qmsPlanEntities.PlanAudit
                                        {
                                            SubProcessId = subprocess.SubProcessId,
                                            SubProcessTitle = subprocess.SubProcessName,
                                            ProcessOwner = audits.DivisionProcesses
                                                .Select(dp => dp.ProcessOwnerId)
                                                .FirstOrDefault()  
                                        })).ToListAsync(),
                };

                return planNull;
            }

            plan = new qmsPlanEntities
            {
                ProcessTitle = processPlan.ProcessTitle,
                PlanId = processPlan.QmsPlans.FirstOrDefault()?.Id,
                AuditObj = processPlan.QmsPlans.FirstOrDefault()?.AuditObj,
                AuditMetho = processPlan.QmsPlans.FirstOrDefault()?.AuditMemo,
                teamLead = processPlan.QmsPlans.FirstOrDefault()?.QmsPlanAudits.FirstOrDefault()?.Team?.TeamLeader,
                membersTeams = processPlan.QmsPlans
                                          .FirstOrDefault()?.QmsPlanAudits
                                          .FirstOrDefault()?.Team?
                                          .QmsteamMembers?.Select(member => new qmsPlanEntities.membersTeam
                                          {
                                              memberId = member.Member,
                                              teamId = member.TeamId
                                          })
                                          .ToList() ?? new List<qmsPlanEntities.membersTeam>(),
                PlanAudits = processPlan.QmsPlans.FirstOrDefault()?.QmsPlanAudits?.Select(audit => new qmsPlanEntities.PlanAudit
                {
                    Id = audit.Id,
                    PlanId = processPlan.QmsPlans.First().Id,
                    SubProcessId = audit.SubProcessId,
                    AuditCriteria = audit.AuditCriteria,
                    ProcessOwner = audit.ProcessOwner,
                    AuditDate = audit.AuditDate,
                    TimeFrom = audit.TimeFrom,
                    TimeTo = audit.TimeTo,
                }).ToList() ?? new List<qmsPlanEntities.PlanAudit>(),
                Status = processPlan.QmsPlans.FirstOrDefault()?.Status,
            };

            return plan;
        }




    }
}
