namespace BSI.SportsLive.DTOs
{
    public class TeamDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string ShortName { get; set; } = string.Empty;
        public string SportType { get; set; } = string.Empty;
        public IEnumerable<PlayerSummaryDto> Players { get; set; } = new List<PlayerSummaryDto>();
    }

    public class TeamCreateDto
    {
        public string Name { get; set; } = string.Empty;
        public string ShortName { get; set; } = string.Empty;
        public string SportType { get; set; } = "Cricket";
    }

    public class TeamUpdateDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string ShortName { get; set; } = string.Empty;
        public string SportType { get; set; } = "Cricket";
    }
}
