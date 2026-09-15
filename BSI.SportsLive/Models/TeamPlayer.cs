using System;

namespace BSI.SportsLive.Models
{
    public class TeamPlayer
    {
        public int Id { get; set; } // Surrogate key — rejoin scenario ke liye zaroori

        public int TeamId { get; set; }
        public Team Team { get; set; } = null!;

        public int PlayerId { get; set; }
        public Player Player { get; set; } = null!;

        public DateTime JoinedDate { get; set; } = DateTime.UtcNow;
        public DateTime? LeftDate { get; set; } // null = abhi bhi is team ka current member hai
    }
}