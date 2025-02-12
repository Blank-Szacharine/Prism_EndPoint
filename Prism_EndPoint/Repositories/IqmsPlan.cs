﻿using Prism_EndPoint.Entities;

namespace Prism_EndPoint.Repositories
{
    public interface IqmsPlan
    {
        Task addQmsPlan(int progId, int divId, int processId);
        Task<IEnumerable<PlanDivisionTeam>> getDivisionPlanTeam(int role, int code, string team);
        Task<IEnumerable<planDivisionTable>> getDivisionprocessPlan(string divisionId, int code);
        Task<qmsPlanEntities> getProcessPlan(int code, int divId, int progId);
        Task updateMetho(int planId, string objectives);
        Task updatePlanAudit(AuditEntry entry);
        Task UpdatePlanObj(int planId, string objectives);
        Task UpdateTeamLead(int planId);
    }
}