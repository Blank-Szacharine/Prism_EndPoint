namespace Prism_EndPoint.Entities
{
    public class FrequencyEntities
    {
        public string? Id { get; set; } = null!;

        public int? divisionId { get; set; } = null!;

        public int? AuditTeam { get; set; } = null!;

        public int? programId { get; set; } = null!;

        public int? processId { get; set; }

        public List<FrequencyMonth> FrequencyMonths { get; set; } = null!;
        public class FrequencyMonth
        {
            public int? month { get; set; } = null!;
            public int? frequencyID { get; set; } = null!;

        }
    }
}
