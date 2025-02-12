using Prism_EndPoint.Models;

namespace Prism_EndPoint.Entities
{
    public class qmsPlanEntities
    {
        public int Id { get; set; }

        public string? ProcessTitle { get; set; }

        public int? PlanId { get; set; }

        public int? checklist {  get; set; }

        public int? divId { get; set; }

        public string? AuditObj { get; set; }

        public string? AuditMetho { get; set; }

        public string? Status { get; set; }

        public string? Approve { get; set; }

        public string? teamLead { get; set; }
        public List<membersTeam> membersTeams { get; set; }
        public class membersTeam
        {

            public string? memberId { get; set; }
            public int? teamId { get; set; }

        }
       

        public List<PlanAudit> PlanAudits { get; set; }
        public class PlanAudit
        {

            public int Id { get; set; }

            public int PlanId { get; set; }

            public int? SubProcessId { get; set; }

            public string? SubProcessTitle { get; set; }

            public string? AuditCriteria { get; set; }

            public string? ProcessOwner { get; set; }

            public int? TeamId { get; set; }

            public int? DivisionId { get; set; }

            public DateOnly? AuditDate { get; set; }

            public TimeOnly? TimeFrom { get; set; }

            public TimeOnly? TimeTo { get; set; }
            public List<auditClauses> auditClause { get; set; } = null!;
            public class auditClauses
            {
                public string subSclauses { get; set; }

            }

        }

    }
}
