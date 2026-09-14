namespace BSI.SportsLive.DTOs
{
    public class TeamSquadDto
    {
        public int TournamentId { get; set; }
        public int TeamId { get; set; }
        public int PlayerId { get; set; }
        public string PlayerName { get; set; } = string.Empty;
        public bool IsPlayingXI { get; set; }
        public bool IsCaptain { get; set; }
        public string? BroadcastNameSnapshot { get; set; }
        public string? ShirtNumberSnapshot { get; set; }
    }

    public class TeamSquadCreateDto
    {
        public int PlayerId { get; set; }
        public bool IsPlayingXI { get; set; } = false;
        public bool IsCaptain { get; set; } = false;
    }

    public class PlayingXIUpdateDto
    {
        public IEnumerable<int> PlayerIds { get; set; } = new List<int>();
    }
}
