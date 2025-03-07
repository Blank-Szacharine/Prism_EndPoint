namespace Prism_EndPoint.Entities
{
    public class updateCheckList
    {
        public List<ChecklistGroup> Checklists { get; set; } = new List<ChecklistGroup>();
        public class ChecklistGroup
        {
            public List<ChecklistItem> Checklists { get; set; } = new List<ChecklistItem>();
        }

        public class ChecklistItem
        {
            public int Id { get; set; }
            public string? Questions { get; set; }  // Mapping 'qa' from payload
            public string? Lookat { get; set; }
            public string? Documentation { get; set; } // Mapping 'document' from payload
            public string? Evidence { get; set; }
            public string? Findings { get; set; }
            public string? Remarks { get; set; }
        }

    }
}
