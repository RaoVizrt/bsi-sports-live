namespace BSI.SportsLive.Models
{
    public class Match
    {
        public int Id { get; set; }
        public string SportType { get; set; } = "Cricket";

        // Nullable — agar tournament ka match hai to value, warna friendly match ke liye null
        public int? TournamentId { get; set; }
        public Tournament? Tournament { get; set; }

        // TeamA/TeamB ab Team entity se linked, strings nahi
        public int TeamAId { get; set; }
        public Team? TeamA { get; set; }

        public int TeamBId { get; set; }
        public Team? TeamB { get; set; }

        // Creator — friendly match ke ownership check ke liye zaroori
        public string UserId { get; set; } = string.Empty;

        public string Status { get; set; } = "Upcoming"; // Upcoming, Live, Completed
        public string LiveScoreSummary { get; set; } = string.Empty;
        public DateTime MatchDate { get; set; }
    }
}