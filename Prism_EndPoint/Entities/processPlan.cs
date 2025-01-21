namespace Prism_EndPoint.Entities
{
    public class processPlan
    {
        public int Id { get; set; }
        public string? division { get; set; }

        public string owner { get; set; } = null!;

        public string title { get; set; } = null!;


        public int ProgramId { get; set; }

        public string? AuditObj { get; set; }

        public string? AuditMemo { get; set; }

        public string? AuditScope { get; set; }

        public int? DivisionId { get; set; }
    }
}
