using BSI.SportsLive.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace BSI.SportsLive.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    // [Authorize(Roles = "Admin")] // Aap baad mein role restriction laga sakte hain
    public class AdminController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public AdminController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        // 1. Get all users with their IsActive status
        [HttpGet("users")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userManager.Users
                .Select(u => new
                {
                    u.Id,
                    u.UserName,
                    u.Email,
                    u.IsActive
                })
                .ToListAsync();

            return Ok(users);
        }

        // 2. Enable or Disable a User
        [HttpPost("toggle-user-status/{id}")]
        public async Task<IActionResult> ToggleUserStatus(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound(new { message = "User nahi mila!" });
            }

            // Status invert kar dein (true ho toh false, false ho toh true)
            user.IsActive = !user.IsActive;
            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            string statusText = user.IsActive ? "enable" : "disable";
            return Ok(new { message = $"User kamyaabi se {statusText} ho gaya hai!", userId = user.Id, isActive = user.IsActive });
        }
    }
}