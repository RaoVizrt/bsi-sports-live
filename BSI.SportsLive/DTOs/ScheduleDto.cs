namespace BSI.SportsLive.DTOs
{
    public class ScheduleDto
    {
        public int Id { get; set; }
        public int TournamentId { get; set; }
        public string TournamentName { get; set; } = string.Empty;
        public int? MatchId { get; set; }
        public int MatchNumber { get; set; }
        public int TeamAId { get; set; }
        public string TeamAName { get; set; } = string.Empty;
        public int TeamBId { get; set; }
        public string TeamBName { get; set; } = string.Empty;
        public string Venue { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public DateTime? StartTime { get; set; }
        public string Status { get; set; } = string.Empty;
    }

    public class ScheduleCreateDto
    {
        public int TournamentId { get; set; }
        public int MatchNumber { get; set; }
        public int TeamAId { get; set; }
        public int TeamBId { get; set; }
        public string Venue { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public DateTime? StartTime { get; set; }
    }

    public class ScheduleUpdateDto
    {
        public int Id { get; set; }
        public int MatchNumber { get; set; }
        public int TeamAId { get; set; }
        public int TeamBId { get; set; }
        public string Venue { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public DateTime? StartTime { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}
