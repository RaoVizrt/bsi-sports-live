using BSI.SportsLive.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BSI.SportsLive.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class MatchesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public MatchesController(AppDbContext context)
        {
            _context = context;
        }

        private string? GetUserId() =>
            User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub");

        // GET: api/matches
        [HttpGet]
        public async Task<IActionResult> GetMatches()
        {
            var userId = GetUserId();
            var isAdmin = User.IsInRole("Admin");

            var query = _context.Matches
                .Include(m => m.TeamA)
                .Include(m => m.TeamB)
                .Include(m => m.Tournament)
                .AsQueryable();

            if (!isAdmin)
            {
                // Tournament ka match ho to tournament owner check, warna match ka apna UserId check
                query = query.Where(m =>
                    (m.TournamentId != null && m.Tournament!.UserId == userId) ||
                    (m.TournamentId == null && m.UserId == userId));
            }

            return Ok(await query.ToListAsync());
        }

        // POST: api/matches
        [HttpPost]
        public async Task<IActionResult> PostMatch(Match match)
        {
            var userId = GetUserId();
            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new { message = "User ID not found in token claims." });

            // Agar tournament-linked match hai, verify user usi tournament ka owner hai
            if (match.TournamentId != null)
            {
                var tournament = await _context.Tournaments.FindAsync(match.TournamentId);
                if (tournament == null) return NotFound(new { message = "Tournament not found." });
                if (tournament.UserId != userId && !User.IsInRole("Admin"))
                    return Forbid();
            }

            match.UserId = userId; // creator hamesha record ho, friendly match ke liye zaroori
            match.Status = string.IsNullOrEmpty(match.Status) ? "Upcoming" : match.Status;

            _context.Matches.Add(match);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetMatches), new { id = match.Id }, match);
        }
    }
}