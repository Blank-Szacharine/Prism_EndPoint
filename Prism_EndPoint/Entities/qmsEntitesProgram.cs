namespace Prism_EndPoint.Entities
{
    public class qmsEntitesProgram
    {
        public int ProgramId { get; set; }

        public string ProgramObj { get; set; } = null!;

        public string ProgramScope { get; set; } = null!;

        public string ProgramMethodology { get; set; } = null!;

        public string ProgramSecV { get; set; } = null!;

        public string ProgramSecVI { get; set; } = null!;

        public string ProgramSecVII { get; set; } = null!;

        public string? QMSleader { get; set; }

        public string? QMSViceleader { get; set; }

        public string? QMSauditLead { get; set; }

        public string? code { get; set; }
        public string? ApprovedQMSLEAD { get; set; }

        public string?ApprovedAuditHead { get; set; }
        public string? Status { get; set; }
        public DateOnly? DateCreated { get; set; }

        public List<FrequencyAudit> FrequencyAudits { get; set; }
        public class FrequencyAudit
        {
            public DateOnly? AuditDate { get; set; }
            public string? TeamLeader { get; set; }

        }

        public List<AuditTeam> AuditTeams { get; set; }
        public class AuditTeam
        {
            public string? members { get; set; }

        }
    }
}
