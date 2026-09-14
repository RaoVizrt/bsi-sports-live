namespace BSI.SportsLive.DTOs
{
    public class PlayingXIPlayerDto
    {
        public int PlayerId { get; set; }
        public string PlayerName { get; set; } = string.Empty;
        public string? BroadcastName { get; set; }
        public string? ShirtNumber { get; set; }
        public int Position { get; set; }
        public bool IsCaptain { get; set; }
    }

    public class PlayingXIUpsertDto
    {
        public int TeamId { get; set; }
        public List<PlayingXIPlayerUpsert> Players { get; set; } = new List<PlayingXIPlayerUpsert>();
    }

    public class PlayingXIPlayerUpsert
    {
        public int PlayerId { get; set; }
        public int Position { get; set; }
        public bool IsCaptain { get; set; }
    }
}
