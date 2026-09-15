using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace BSI.SportsLive.Models
{
    public class Player
    {
        public int Id { get; set; }

        [Required]
        public string FullName { get; set; } = string.Empty;

        [Required]
        public string BroadcastName { get; set; } = string.Empty;

        public string ShirtNumber { get; set; } = string.Empty;

        public string? PhotoUrl { get; set; }

        // Navigation property for Many-to-Many with Teams
        public ICollection<TeamPlayer> TeamPlayers { get; set; } = new List<TeamPlayer>();
    }
}