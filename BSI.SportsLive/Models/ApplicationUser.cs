using Microsoft.AspNetCore.Identity;

namespace BSI.SportsLive.Models
{
    public class ApplicationUser : IdentityUser
    {
        // Account status for tournament rental management (true = active, false = disabled/blocked)
        public bool IsActive { get; set; } = true;

        // Additional profile properties can be added here
    }
}