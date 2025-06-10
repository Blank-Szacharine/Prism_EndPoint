namespace Prism_EndPoint.Entities
{
    public class ClauseReportDto
    {
   
        public int auditedProcess { get; set; } = 0;
        public int allProcess { get; set; } = 0;
        public List<ArrayInput> ArrayInputs { get; set; } = null!;
        public List<division> divisions { get; set; } = null!;

        public class division
        {
          public int divisionId { get; set; }

            public List<divisionProcess> divisionprocess { get; set; } = null!;
            public class divisionProcess
            {
                public string clauseId { get; set; }
                public string ClauseName { get; set; }
                public int ComplianceCount { get; set; }
                public int NonComplianceCount { get; set; }
                public int OfiCount { get; set; }
            }
        }


        public class ArrayInput
        {
            public string divisionId { get; set; }
            public string clauseId { get; set; }
            public string ClauseName { get; set; }
            public int ComplianceCount { get; set; }
            public int NonComplianceCount { get; set; }
            public int OfiCount { get; set; }
        }
    }
}
