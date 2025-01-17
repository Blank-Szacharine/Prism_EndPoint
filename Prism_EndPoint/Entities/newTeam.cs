namespace Prism_EndPoint.Entities
{
    public class newTeam
    {
        public string? teamleader { get; set; }
        public int? teamID { get; set; }
        public List<membersTeam> membersTeams { get; set; }
        public class membersTeam
        {
            
            public string? memberId { get; set; }
            public int? teamId { get; set; }

        }
    }
}
