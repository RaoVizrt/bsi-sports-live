namespace BSI.SportsLive.Models
{
    // Represents a player assigned to a team in a specific tournament (squad / playing XI)
    public class TeamSquad
    {
        public int TournamentId { get; set; }
        public Tournament? Tournament { get; set; }

        public int TeamId { get; set; }
        public Team? Team { get; set; }

        public int PlayerId { get; set; }
        public Player? Player { get; set; }

        // Squad role flags
        public bool IsPlayingXI { get; set; } = false;
        public bool IsCaptain { get; set; } = false;

        // Optional snapshot fields
        public string? BroadcastNameSnapshot { get; set; }
        public string? ShirtNumberSnapshot { get; set; }
    }
}
