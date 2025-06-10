using Microsoft.EntityFrameworkCore;
using Prism_EndPoint.Entities;
using Prism_EndPoint.Models;
using System.Collections.Generic;

namespace Prism_EndPoint.Repositories
{
    public class qmsReport : IqmsReport
    {
        private readonly PrismDbContext _QmsContext;
        public qmsReport(PrismDbContext QmsContext)
        {
            _QmsContext = QmsContext;
        }


        public async Task<ClauseReportDto> GetReportAsync(string code)
        {


            try
            {
                var countProcess = _QmsContext.DivisionProcesses
                    .Distinct()
                    .Count();


                var progRams = await _QmsContext.QmsPrograms
                    .Include(x => x.QmsPlans)
                        .ThenInclude(p => p.QmsCheckLists)
                    .Where(x => x.Code == code)
                    .FirstOrDefaultAsync();

                if (progRams == null) return new ClauseReportDto();

                var allChecklistIds = progRams.QmsPlans
                    .SelectMany(p => p.QmsCheckLists)
                    .Select(cl => cl.Id)
                    .ToList();
                var auditedChecklistCount = allChecklistIds.Count;
                
                var parameterizedQuery = string.Join(",", allChecklistIds.Select((id, index) => $"@p{index}"));

               
                var query = $"SELECT * FROM QmsCheckListAudit WHERE CheckListId IN ({parameterizedQuery})";

               
                var audits = await _QmsContext.QmsCheckListAudits
                    .FromSqlRaw(query, allChecklistIds.Cast<object>().ToArray())
                    .ToListAsync();

                var groupedByClause = audits
                    .GroupBy(a => a.ClauseId)
                    .Select(g => new
                    {
                        ClauseId = g.Key,
                        ComplianceCount = g.Count(x => x.Findings == "C"),
                        NonComplianceCount = g.Count(x => x.Findings == "NC"),
                        OfiCount = g.Count(x => x.Findings == "OFI")
                    })
                    .ToList();

                var clauses = await _QmsContext.QmsSubClauses.ToListAsync();

                var report = groupedByClause
                    .Join(clauses, g => g.ClauseId, c => c.Id, (g, c) => new ClauseReportDto.ArrayInput
                    {
                        clauseId = c.Subclause,
                        ClauseName = c.ClauseTitle ?? c.SubTitle,
                        ComplianceCount = g.ComplianceCount,
                        NonComplianceCount = g.NonComplianceCount,
                        OfiCount = g.OfiCount
                    })
                    .ToList();


                var divisionArray = new List<ClauseReportDto.division>();
                for (int id = 1; id <= 11; id++)
                {
                    var demo = await _QmsContext.QmsPrograms
                    .Include(x => x.QmsPlans)
                        .ThenInclude(p => p.QmsCheckLists)
                    .Where(x => x.QmsPlans.Any(p => p.DivisionId == id))
                    .FirstOrDefaultAsync();


                    if (demo != null)
                    {
                        var allChecklistIdsDemo = progRams.QmsPlans
                            .Where(cl => cl.DivisionId == id)
                            .SelectMany(p => p.QmsCheckLists)
                            .Select(cl => cl.Id)
                            .ToList();

                        if (allChecklistIdsDemo.Count != 0)
                        {
                            var parameterizedQueryDemo = string.Join(",", allChecklistIdsDemo.Select((id, index) => $"@p{index}"));

                            var queryDemo = $"SELECT * FROM QmsCheckListAudit WHERE CheckListId IN ({parameterizedQueryDemo})";

                            var auditsDemo = await _QmsContext.QmsCheckListAudits
                                .FromSqlRaw(queryDemo, allChecklistIdsDemo.Cast<object>().ToArray())
                                .ToListAsync();

                            var groupedByClauseDemo = auditsDemo
                                 .GroupBy(a => a.ClauseId)
                                 .Select(g => new
                                 {
                                     ClauseId = g.Key,
                                     ComplianceCount = g.Count(x => x.Findings == "C"),
                                     NonComplianceCount = g.Count(x => x.Findings == "NC"),
                                     OfiCount = g.Count(x => x.Findings == "OFI")
                                 })
                                 .ToList();

                            var clausesDemo = await _QmsContext.QmsSubClauses.ToListAsync();

                            var divisionInputs = groupedByClauseDemo
                                .Join(clausesDemo, g => g.ClauseId, c => c.Id, (g, c) => new
                                {
                                    Clause = c,
                                    Group = g
                                })
                                .GroupBy(x => id)
                                .Select(g => new ClauseReportDto.division
                                {
                                    divisionId = g.Key,
                                    divisionprocess = g.Select(x => new ClauseReportDto.division.divisionProcess
                                    {
                                        clauseId = x.Clause.Subclause,
                                        ClauseName = x.Clause.ClauseTitle ?? x.Clause.SubTitle,
                                        ComplianceCount = x.Group.ComplianceCount,
                                        NonComplianceCount = x.Group.NonComplianceCount,
                                        OfiCount = x.Group.OfiCount
                                    }).ToList()
                                })
                                .ToList();

                            divisionArray.AddRange(divisionInputs);
                        }
                    }else
                    {
                       
                    }
                
                
                }
                



                return new ClauseReportDto
                {
                    auditedProcess = auditedChecklistCount,
                    allProcess = countProcess,
                    divisions = divisionArray,
                    ArrayInputs = report
                };
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error generating QMS report", ex);
            }







        }


    }
}
