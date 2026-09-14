namespace BSI.SportsLive.Models
{
    // Snapshot of Playing XI for a team in a match
    public class MatchPlayingXI
    {
        public int MatchId { get; set; }
        public Match? Match { get; set; }

        public int TeamId { get; set; }
        public Team? Team { get; set; }

        public int PlayerId { get; set; }
        public Player? Player { get; set; }

        // Position/order in XI (1-11)
        public int Position { get; set; }

        public bool IsCaptain { get; set; }

        // Snapshots to preserve display values
        public string? BroadcastNameSnapshot { get; set; }
        public string? ShirtNumberSnapshot { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
