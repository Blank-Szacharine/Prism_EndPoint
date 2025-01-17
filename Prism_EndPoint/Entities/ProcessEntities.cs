namespace Prism_EndPoint.Entities
{
    public class ProcessEntities
    {
        public string? division { get; set; }

        public string owner { get; set; } = null!;

        public string title { get; set; } = null!;

        public string description { get; set; } = null!;
        public string? ProcessFile { get; set; }

        public string? ProcessDocNum { get; set; }
    }
}
