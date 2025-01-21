using Prism_EndPoint.Entities;

namespace Prism_EndPoint.Repositories
{
    public interface IqmsPlan
    {
        Task<IEnumerable<PlanDivisionTeam>> getDivisionPlanTeam(int code, string team);
        Task<IEnumerable<planDivisionTable>> getDivisionprocessPlan(string divisionId, int code);
        Task<qmsPlanEntities> getProcessPlan(int code, int divId, int progId);
    }
}