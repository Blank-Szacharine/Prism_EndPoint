namespace Prism_EndPoint.Entities
{
    public class AuditEntry
    {
        public int planId { get; set; }
        public int subprocId { get; set; } 
        public string criteria { get; set; }
        public DateOnly auditDate { get; set; }
        public TimeOnly from { get; set; }
        public TimeOnly to { get; set; }

        public List<auditClauses> auditClause { get; set; } = null!;
        public class auditClauses
        {
            public string subSclauses { get; set; }

        }
    }
}
