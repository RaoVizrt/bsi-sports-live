using BSI.SportsLive.Data;
using BSI.SportsLive.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BSI.SportsLive.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TournamentsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TournamentsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/tournaments
        [HttpGet]
        public async Task<IActionResult> GetTournaments()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)
                         ?? User.FindFirstValue("sub")
                         ?? User.FindFirstValue("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");

            var isAdmin = User.IsInRole("Admin");

            List<Tournament> tournaments;

            if (isAdmin || string.IsNullOrEmpty(userId))
            {
                tournaments = await _context.Tournaments.ToListAsync();
            }
            else
            {
                tournaments = await _context.Tournaments
                    .Where(t => t.UserId == userId)
                    .ToListAsync();
            }

            return Ok(tournaments);
        }

        // GET: api/tournaments/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTournament(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)
                         ?? User.FindFirstValue("sub")
                         ?? User.FindFirstValue("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");

            var isAdmin = User.IsInRole("Admin");

            var tournament = await _context.Tournaments
                .Include(t => t.TournamentTeams)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (tournament == null)
            {
                return NotFound();
            }

            if (!isAdmin && tournament.UserId != userId)
            {
                return Forbid();
            }

            return Ok(tournament);
        }

        // POST: api/tournaments
        [HttpPost]
        public async Task<IActionResult> CreateTournament([FromBody] Tournament tournament)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)
                         ?? User.FindFirstValue("sub")
                         ?? User.FindFirstValue("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new { message = "User ID not found in token claims." });
            }

            tournament.UserId = userId;
            tournament.Status = string.IsNullOrEmpty(tournament.Status) ? "Upcoming" : tournament.Status;

            _context.Tournaments.Add(tournament);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTournament), new { id = tournament.Id }, tournament);
        }
    }
}