using Prism_EndPoint.Entities;

namespace Prism_EndPoint.Repositories
{
    public interface IqmsPlan
    {
        Task addQmsPlan(int progId, int divId, int processId);
        Task<IEnumerable<PlanDivisionTeam>> getDivisionPlanTeam(int role, int code, string team);
        Task<IEnumerable<planDivisionTable>> getDivisionprocessPlan(string divisionId, int code, int role, string empno, string programId);
        Task<qmsPlanEntities> getProcessPlan(int code, int divId, int progId);
        Task<List<qmsPlanEntities>> PrintProcessPlanAsync(int divId, int progId);
        Task returnPlan(int planId, string Conclusion);
        Task updateMetho(int planId, string objectives);
        Task updatePlanAudit(AuditEntry entry);
        Task UpdatePlanObj(int planId, string objectives);
        Task UpdateTeamLead(int planId);
    }
}