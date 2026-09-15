using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using BSI.SportsLive.Models;
using BSI.SportsLive.DTOs;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BSI.SportsLive.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _config;

        public AccountController(UserManager<ApplicationUser> userManager, IConfiguration config)
        {
            _userManager = userManager;
            _config = config;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            var user = new ApplicationUser { UserName = dto.UserName, Email = dto.Email, IsActive = true };
            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded) return BadRequest(result.Errors);
            return Ok(new { message = "User created successfully" });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var user = await _userManager.FindByNameAsync(dto.UserName);
            if (user == null) return Unauthorized(new { message = "Invalid username or password." });

            // Check if account is active/enabled for tournament rentals
            if (!user.IsActive)
            {
                return Unauthorized(new { message = "Aapka account disable ya tournament rental khatam ho chuka hai. Admin se rabta karein!" });
            }

            if (!await _userManager.CheckPasswordAsync(user, dto.Password)) return Unauthorized(new { message = "Invalid username or password." });

            var token = await GenerateTokenAsync(user);
            return Ok(new { token });
        }

        private async Task<string> GenerateTokenAsync(ApplicationUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName ?? string.Empty)
            };

            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = _config["Jwt:Key"] ?? throw new InvalidOperationException("JWT key not configured");
            var issuer = _config["Jwt:Issuer"] ?? "BSI.SportsLive";
            var audience = _config["Jwt:Audience"] ?? "BSI.SportsLiveClients";
            var expiresMinutes = int.TryParse(_config["Jwt:DurationMinutes"], out var m) ? m : 60;

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var creds = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expiresMinutes),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}