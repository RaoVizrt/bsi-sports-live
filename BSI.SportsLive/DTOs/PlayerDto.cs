namespace BSI.SportsLive.DTOs
{
    public class TeamMembershipDto
    {
        public int TeamId { get; set; }
        public string TeamName { get; set; } = string.Empty;
        public DateTime JoinedDate { get; set; }
        public DateTime? LeftDate { get; set; }
    }

    public class PlayerDto
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string BroadcastName { get; set; } = string.Empty;
        public string ShirtNumber { get; set; } = string.Empty;
        public string? PhotoUrl { get; set; }
        public List<TeamMembershipDto> CurrentTeams { get; set; } = new();
        public List<TeamMembershipDto> PastTeams { get; set; } = new();
    }

    public class PlayerCreateDto
    {
        public string FullName { get; set; } = string.Empty;
        public string? BroadcastName { get; set; }
        public string? ShirtNumber { get; set; }
        public string? PhotoUrl { get; set; }
        // TeamId yahan se hata diya — player creation ab team-independent hai
    }

    public class PlayerUpdateDto
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string? BroadcastName { get; set; }
        public string? ShirtNumber { get; set; }
        public string? PhotoUrl { get; set; }
    }

    public class PlayerSummaryDto
    {
        public int Id { get; set; }
        public string BroadcastName { get; set; } = string.Empty;
        public string ShirtNumber { get; set; } = string.Empty;
        public string? PhotoUrl { get; set; }
    }
}