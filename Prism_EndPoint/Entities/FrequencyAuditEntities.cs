using static Prism_EndPoint.Entities.FrequencyAuditEntities.ArrayInput;

namespace Prism_EndPoint.Entities
{
    public class FrequencyAuditEntities
    {
        public int? codeProgram { get; set; } = null!;

        public List<ArrayInput> ArrayInputs { get; set; } = null!;
        public class ArrayInput
        {
            public string? Id { get; set; } = null!;

            public int? divisionId { get; set; } = null!;

            public int? AuditTeam { get; set; } = null!;

            public int? programId { get; set; } = null!;

            public List<FrequencyMonth> FrequencyMonths { get; set; } = null!;
            public class FrequencyMonth
            {
                public int? month { get; set; } = null!;
                public int? frequencyID { get; set; } = null!;

            }
        }

        
    }
}
