namespace BSI.SportsLive.Models
{
    public class TournamentTeam
    {
        public int TournamentId { get; set; }
        public Tournament? Tournament { get; set; }

        public int TeamId { get; set; }
        public Team? Team { get; set; }

        // Optional squad snapshot fields
        public string? Role { get; set; }
    }
}
