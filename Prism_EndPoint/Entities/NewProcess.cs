namespace Prism_EndPoint.Entities
{
    public class NewProcess
    {
        public string? division { get; set; }

        public string owner { get; set; } = null!;

        public string title { get; set; } = null!;

        public string description { get; set; } = null!;
        public List<subProcess> SubProcess { get; set; }
        public class subProcess
        {

            public string? ProcessTitle { get; set; }

        }

    }
}
