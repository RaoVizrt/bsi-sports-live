using System.ComponentModel.DataAnnotations;
using System.Numerics;

namespace BSI.SportsLive.Models
{
    public class Team
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty; // Full Name (e.g., Pakistan)

        [Required]
        [MaxLength(10)]
        public string ShortName { get; set; } = string.Empty; // Broadcast Code (e.g., PAK)

        public string SportType { get; set; } = "Cricket"; // Cricket, Football, etc.

        // Navigation property for players belonging to this team
        public ICollection<Player> Players { get; set; } = new List<Player>();
    }
}