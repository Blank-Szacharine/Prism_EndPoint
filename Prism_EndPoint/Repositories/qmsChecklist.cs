using Microsoft.EntityFrameworkCore;
using Prism_EndPoint.Entities;
using Prism_EndPoint.Models;
using System.Diagnostics;

namespace Prism_EndPoint.Repositories
{
    public class qmsChecklist : IqmsChecklist
    {
        private readonly PrismDbContext _QmsContext;
        public qmsChecklist(PrismDbContext QmsContext)
        {
            _QmsContext = QmsContext;
        }

        public async Task<getChecklist> getCheckListItem(int code)
        {

            var getCheck = await _QmsContext.QmsPlans
                                .Include(x => x.QmsPlanAudits)
                                .Include(x => x.QmsCheckLists)
                                    .ThenInclude(x => x.QmsCheckListAudits)
                                    .Where(x => x.Id == code)
                                .FirstOrDefaultAsync();


            if (getCheck == null || getCheck.QmsCheckLists == null || !getCheck.QmsCheckLists.Any())
            {
                return null;
            }


            var firstCheckList = getCheck.QmsCheckLists.FirstOrDefault();


            if (firstCheckList == null)
            {
                return null;
            }


            var checkList = new getChecklist
            {
                Id = firstCheckList.Id,
                AuditId = getCheck.Id,
                auditTitle = _QmsContext.Qmsprocesses.Where(x => x.Id == getCheck.ProcessId).FirstOrDefault().ProcessTitle,
                AuditedBy = _QmsContext.Qmsteams.Where(x => x.Id == getCheck.QmsPlanAudits.FirstOrDefault().TeamId).FirstOrDefault().TeamLeader,
                AuditedDate = firstCheckList.AuditedDate,
                AcknowledgeBy = firstCheckList.AcknowledgeBy,
                Status = firstCheckList.Status,
                DivisionId = getCheck.DivisionId,
                Owner = _QmsContext.DivisionProcesses.Where(x => x.ProcessId == getCheck.ProcessId).FirstOrDefault().ProcessOwnerId,

                AcknowledgeDate = firstCheckList.AcknowledgeDate,
                auditLists = getCheck.QmsPlanAudits.Select(audits => new getChecklist.auditList
                {
                    auditsubTitle = _QmsContext.QmssubProcesses.Where(x => x.SubProcessId == audits.SubProcessId).FirstOrDefault().SubProcessName,
                    AuditDate = audits.AuditDate,
                    TimeFrom = audits.TimeFrom,
                    TimeTo = audits.TimeTo,
                    checkListLists = audits.QmsCheckListAudits?.Select(list => new getChecklist.auditList.checkListList
                    {
                        Id = list.Id,
                        CheckListId = list.CheckListId,
                        checkListClause = list.ClauseId != null
                         ? _QmsContext.QmsSubClauses
                             .Where(x => x.Id == list.ClauseId)
                             .Select(x => x.Subclause)
                             .FirstOrDefault()
                         : null,
                        AuditPlanId = list.AuditPlanId,
                        Questions = list.Questions,
                        Documentation = list.Documentation,
                        Evidence = list.Evidence,
                        Findings = list.Findings,
                        clause = list.ClauseId,
                        clauseTitle = list.ClauseId != null
                         ? _QmsContext.QmsSubClauses
                             .Where(x => x.Id == list.ClauseId)
                             .Select(x => x.SubTitle ?? x.ClauseTitle)
                             .FirstOrDefault()
                         : null
                    }).ToList()

                }).ToList(),

            };

            return checkList;
        }


        public async Task createCheckList(int code)
        {
            var planAudit = await _QmsContext.QmsPlans
                                .Include(x => x.QmsPlanAudits)
                                .ThenInclude(c => c.QmsPlanAuditClauses)
                                .Where(x => x.Id == code)
                                .FirstOrDefaultAsync();
            var chechker = _QmsContext.QmsCheckLists.Where(x => x.AuditId == code).FirstOrDefault();

            if (chechker != null)
            {
                return;
            }

            var checklist = new QmsCheckList
            {
                AuditId = planAudit.Id,
                AuditedBy = _QmsContext.Qmsteams.Where(x => x.Id == planAudit.QmsPlanAudits.FirstOrDefault().TeamId).FirstOrDefault().TeamLeader,
                AuditedDate = null,
                AcknowledgeBy = _QmsContext.PrismCredentials.FirstOrDefault().AuditHead,
                AcknowledgeDate = null,
                Status = "Drafted",
            };
            await _QmsContext.QmsCheckLists.AddAsync(checklist);
            await _QmsContext.SaveChangesAsync();

            foreach (var auditChecklist in planAudit.QmsPlanAudits)
            {

                foreach (var auditClauses in auditChecklist.QmsPlanAuditClauses)
                {
                    var checkAudit = new QmsCheckListAudit
                    {
                        CheckListId = checklist.Id,
                        AuditPlanId = auditChecklist.Id,
                        Questions = null,
                        Documentation = null,
                        Evidence = null,
                        ClauseId = _QmsContext.QmsSubClauses.SingleOrDefault(x => x.Subclause == auditClauses.SubClause).Id
                    };

                    await _QmsContext.QmsCheckListAudits.AddAsync(checkAudit);
                    await _QmsContext.SaveChangesAsync();
                }

            }

        }


        public async Task updateCheckListData(updateCheckList update)
        {
            foreach (var list in update.Checklists)
            {
                foreach (var item in list.Checklists)
                {
                    var auditChecklist = await _QmsContext.QmsCheckListAudits
                                                .Where(x => x.Id == item.Id)
                                                .FirstOrDefaultAsync();


                    if (auditChecklist != null)
                    {
                        auditChecklist.Questions = item.Questions;
                        auditChecklist.Documentation = item.Documentation;
                        auditChecklist.Evidence = item.Evidence;
                        auditChecklist.Findings = item.Findings;
                        _QmsContext.QmsCheckListAudits.Update(auditChecklist);
                        await _QmsContext.SaveChangesAsync();
                    }
                }
            }
        }

        public async Task updateStatus(int id)
        {
            var checklist = _QmsContext?.QmsCheckLists.Where(x => x.Id == id).FirstOrDefault();
            var status = "";

            if (checklist != null)
            {
                if (checklist.Status == "Drafted")
                {
                    status = "AuditorFinalaize";
                    checklist.Status = status;
                    checklist.AuditedDate = DateOnly.FromDateTime(DateTime.Today);
                    _QmsContext.QmsCheckLists.Update(checklist);
                    await _QmsContext.SaveChangesAsync();
                }
                else
                {
                    status = "Submitted";
                    checklist.Status = status;
                    checklist.AcknowledgeDate = DateOnly.FromDateTime(DateTime.Today);
                    _QmsContext.QmsCheckLists.Update(checklist);
                    await _QmsContext.SaveChangesAsync();
                }


            }
        }


        public async Task<ReportEntities> GetChecklistReport(int code)
        {

            var reportList = await _QmsContext.QmsAuditReports.Where(x => x.ChecklistId == code).FirstOrDefaultAsync();

            var qmsProgram = await _QmsContext.QmsPrograms
                                .Include(x => x.QmsPlans)
                                    .ThenInclude(z => z.QmsCheckLists)
                                .Where(x => x.QmsPlans.Any(p => p.QmsCheckLists.Any(c => c.Id == code)))
                                .FirstOrDefaultAsync();

            var membersAudit = await _QmsContext.FrequencyAudits
                                        .Include(x => x.AuditTeamNavigation)
                                            .ThenInclude(x => x.QmsteamMembers)
                                        .Where(x => x.ProgramId == qmsProgram.Id)
                                        .FirstOrDefaultAsync();
            var ownerProcess = await _QmsContext.Qmsprocesses
                                            .Include(x => x.DivisionProcesses)
                                            .Where(x => x.Id == qmsProgram.QmsPlans.FirstOrDefault().ProcessId)
                                            .FirstOrDefaultAsync();


            if (reportList == null)
            {
                var newReport = new QmsAuditReport
                {
                    ChecklistId = code,
                };
                await _QmsContext.QmsAuditReports.AddAsync(newReport);
                await _QmsContext.SaveChangesAsync();
                reportList = await _QmsContext.QmsAuditReports.Where(x => x.ChecklistId == code).FirstOrDefaultAsync();

            }
            var qmsplanAudit = qmsProgram.QmsPlans?.FirstOrDefault()?.Id;



            var report = new ReportEntities
            {
                Id = code,
                AuditArea = reportList.AuditArea,
                Date = _QmsContext.QmsPlanAudits
                .Where(x => x.PlanId == qmsplanAudit)
                .FirstOrDefault()?.AuditDate,
                PGPIdentified = reportList.Pgpidentified,
                NONConfe = reportList.NonConfirmities,
                OFI = reportList.Ofi,
                nextAction = reportList.NextAction,
                ProcessTitle = ownerProcess.ProcessTitle,
                Division = ownerProcess.DivisionProcesses.FirstOrDefault()?.DivisionId,
                Conclusion = reportList.Conclusion,
                AuditorSigned = reportList.AuditorSigned,
                ViceAuditorSigned = reportList.ViceAuditorSigned,
                AuditeeSigned = reportList.AuditeeSigned,
                Owner = ownerProcess.DivisionProcesses.FirstOrDefault()?.ProcessOwnerId,
                Leader = membersAudit.AuditTeamNavigation.TeamLeader,
                TeamMembers = membersAudit.AuditTeamNavigation.QmsteamMembers?.Select(member => new ReportEntities.Team
                {
                    Members = member.Member
                }).ToList(),
                Results = _QmsContext.QmsCheckListAudits.Where(x => x.CheckListId == code).Select(result => new ReportEntities.result
                {
                    Findings = result.Findings,
                    ClauseTitle = _QmsContext.QmsSubClauses
                                .Where(x => x.Id == result.ClauseId)
                                .Select(x => x.ClauseTitle ?? x.SubTitle)
                                .FirstOrDefault(),
                    ClauseId = _QmsContext.QmsSubClauses.Where(x => x.Id == result.ClauseId).FirstOrDefault().Subclause,
                }).ToList(),
            };

            return report;

        }

        public async Task updateReport(ReportUpdateData update)
        {
            var reporT = await _QmsContext.QmsAuditReports.Where(x => x.ChecklistId == update.ChecklistId).FirstOrDefaultAsync();

            reporT.AuditArea = update.AuditArea;
            reporT.Pgpidentified = update.Pgpidentified;
            reporT.NonConfirmities = update.NonConfirmities;
            reporT.Ofi = update.Ofi;
            reporT.NextAction = update.nextAction;
            reporT.Conclusion = update.Conclusion;
            _QmsContext.QmsAuditReports.Update(reporT);
            await _QmsContext.SaveChangesAsync();
        }




    }
}
