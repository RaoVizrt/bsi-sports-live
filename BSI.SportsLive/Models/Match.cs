namespace BSI.SportsLive.Models
{
    public class Match
    {
        public int Id { get; set; }
        public string SportType { get; set; } = "Cricket"; // Jaise Cricket, Football, etc.
        public string TournamentName { get; set; } = string.Empty;
        public string TeamA { get; set; } = string.Empty;
        public string TeamB { get; set; } = string.Empty;
        public string Status { get; set; } = "Upcoming"; // Upcoming, Live, Completed
        public string LiveScoreSummary { get; set; } = string.Empty; // vMix ya JSON output ke liye
        public DateTime MatchDate { get; set; }
    }
}