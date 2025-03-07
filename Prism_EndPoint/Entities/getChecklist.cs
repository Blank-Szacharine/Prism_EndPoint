using Prism_EndPoint.Models;

namespace Prism_EndPoint.Entities
{
    public class getChecklist
    {
        public int Id { get; set; }

        public int AuditId { get; set; }

        public string? auditTitle { get; set; }
        public string? AuditedBy { get; set; }

        public int? DivisionId { get; set; }

        public string? Status { get; set; }
        public string? Owner { get; set; }
        public DateOnly? AuditedDate { get; set; }

        public string? AcknowledgeBy { get; set; }

        public DateOnly? AcknowledgeDate { get; set; }

        public DateOnly? ReviewedDate { get; set; }
        public string? ReviewedBy { get; set; }

        public List<Auditee> auditees { get; set; }

        public class Auditee
        {
            public string? member { get; set; }

        }
        public List<auditList> auditLists { get; set; }
        public class auditList
        {
            public string? auditsubTitle { get; set; }
            public DateOnly? AuditDate { get; set; }

            public TimeOnly? TimeFrom { get; set; }

            public TimeOnly? TimeTo { get; set; }
            public List<checkListList> checkListLists { get; set; }
            public class checkListList
            {
                public int Id { get; set; }

                public int CheckListId { get; set; }

                public string? checkListClause { get; set; }
                public int AuditPlanId { get; set; }

                public string? Questions { get; set; }
                public string? LookAt { get; set; }

                public string? Documentation { get; set; }

                public string? Evidence { get; set; }

                public string? Findings { get; set; }

                public string? Remarks { get; set; }

                public int? clause { get; set; }

                public string? clauseTitle { get; set; }
            }

        }


       
    }
}
