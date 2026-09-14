using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BSI.SportsLive.Models
{
    public class Player
    {
        public int Id { get; set; }

        [Required]
        public string FullName { get; set; } = string.Empty; // Complete Name (can have 2, 3, or 4+ parts)

        [Required]
        public string BroadcastName { get; set; } = string.Empty; // Name for Graphics (e.g., Babar A.)

        public string ShirtNumber { get; set; } = string.Empty; // Jersey Number

        // Foreign Key to Team
        public int TeamId { get; set; }

        [ForeignKey("TeamId")]
        public Team? Team { get; set; }
    }
}