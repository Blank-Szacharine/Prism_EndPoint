namespace Prism_EndPoint.Entities
{
    public class qmsClauses
    {
        public int? Clauses { get; set; } = null!;
        public string? Title { get; set; } = null!;
        public List<ArrayInput> subClause { get; set; } = null!;
        public class ArrayInput
        {
            public string? subClause { get; set; } = null!;

            public string? Title { get; set; } = null!;

            public string? subTitle { get; set; } = null!;


            public string? status { get; set; } = null!;
        }
    }
}
