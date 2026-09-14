namespace BSI.SportsLive.DTOs
{
    public class TournamentDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string ShortName { get; set; } = string.Empty;
        public string Season { get; set; } = string.Empty;
        public string LogoUrl { get; set; } = string.Empty;
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Format { get; set; } = "T20";
        public int OversPerInnings { get; set; }
        public string Status { get; set; } = string.Empty;
        public IEnumerable<TournamentTeamDto> TournamentTeams { get; set; } = new List<TournamentTeamDto>();
    }

    public class TournamentCreateDto
    {
        public string Name { get; set; } = string.Empty;
        public string ShortName { get; set; } = string.Empty;
        public string Season { get; set; } = string.Empty;
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Format { get; set; } = "T20";
        public int OversPerInnings { get; set; } = 20;
    }

    public class TournamentUpdateDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string ShortName { get; set; } = string.Empty;
        public string Season { get; set; } = string.Empty;
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Format { get; set; } = "T20";
        public int OversPerInnings { get; set; } = 20;
        public string Status { get; set; } = string.Empty;
    }

    public class TournamentTeamDto
    {
        public int TournamentId { get; set; }
        public int TeamId { get; set; }
        public string TeamName { get; set; } = string.Empty;
        public string? Role { get; set; }
    }
}
