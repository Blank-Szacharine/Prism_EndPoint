namespace Prism_EndPoint.Entities
{
    public class ReportEntities
    {
        public int? Id { get; set; }
        public DateOnly? Date { get; set; }
        public string? Auditee { get; set; }
        public string? AuditArea { get; set; }

        public string? nextAction { get; set; }
        public string? PGPIdentified { get; set; }

        public string? NONConfe {  get; set; }
        public string? OFI {  get; set; }
        public string? Division { get; set; }
        public string? ProcessTitle { get; set; }
        public string? Conclusion { get; set; }
        public DateOnly? AuditorSigned { get; set; }
        public DateOnly? ViceAuditorSigned { get; set; }
        public DateOnly? AuditeeSigned { get; set; }


        public string? Owner { get; set; }
        public string? Leader { get; set; }
        public List<Team>? TeamMembers { get; set; } = null!;
        public class Team
        {
            public string Members { get; set; }

        }

        public List<result>? Results { get; set; } = null!;
        public class result
        {
            public string? Findings { get; set; }

            public string? ClauseId { get; set; }

            public string? ClauseTitle { get; set; }

            public string? Remarks { get; set; }
        }

    }
}
