namespace BSI.SportsLive.DTOs
{
    public class PlayerDto
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string BroadcastName { get; set; } = string.Empty;
        public string ShirtNumber { get; set; } = string.Empty;
        public int TeamId { get; set; }
        public string TeamName { get; set; } = string.Empty;
    }

    public class PlayerCreateDto
    {
        public string FullName { get; set; } = string.Empty;
        public string? BroadcastName { get; set; }
        public string? ShirtNumber { get; set; }
        public int TeamId { get; set; }
    }

    public class PlayerUpdateDto
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string? BroadcastName { get; set; }
        public string? ShirtNumber { get; set; }
        public int TeamId { get; set; }
    }

    public class PlayerSummaryDto
    {
        public int Id { get; set; }
        public string BroadcastName { get; set; } = string.Empty;
        public string ShirtNumber { get; set; } = string.Empty;
    }
}
