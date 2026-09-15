using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BSI.SportsLive.Models;
using BSI.SportsLive.DTOs;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;

namespace BSI.SportsLive.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class TeamsController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly IMapper _mapper;

        public TeamsController(AppDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var teams = await _db.Teams
                .Include(t => t.TeamPlayers)
                .ThenInclude(tp => tp.Player)
                .ToListAsync();
            var dto = _mapper.Map<IEnumerable<TeamDto>>(teams);
            return Ok(dto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var team = await _db.Teams
                .Include(t => t.TeamPlayers)
                .ThenInclude(tp => tp.Player)
                .FirstOrDefaultAsync(t => t.Id == id);
            if (team == null) return NotFound();
            var dto = _mapper.Map<TeamDto>(team);
            return Ok(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TeamCreateDto teamCreate)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var team = _mapper.Map<Team>(teamCreate);
            _db.Teams.Add(team);
            await _db.SaveChangesAsync();
            var dto = _mapper.Map<TeamDto>(team);
            return CreatedAtAction(nameof(Get), new { id = team.Id }, dto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] TeamUpdateDto updated)
        {
            if (id != updated.Id) return BadRequest();
            var team = await _db.Teams.FindAsync(id);
            if (team == null) return NotFound();
            _mapper.Map(updated, team);
            await _db.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var team = await _db.Teams.FindAsync(id);
            if (team == null) return NotFound();
            _db.Teams.Remove(team);
            await _db.SaveChangesAsync();
            return NoContent();
        }

        // POST: api/teams/5/players/12 — existing global player ko is team mein add karo
        [HttpPost("{teamId}/players/{playerId}")]
        public async Task<IActionResult> AddPlayerToTeam(int teamId, int playerId)
        {
            var team = await _db.Teams.FindAsync(teamId);
            if (team == null) return NotFound("Team not found");

            var player = await _db.Players.FindAsync(playerId);
            if (player == null) return NotFound("Player not found");

            // Check karo already current member to nahi hai
            var alreadyActive = await _db.TeamPlayers
                .AnyAsync(tp => tp.TeamId == teamId && tp.PlayerId == playerId && tp.LeftDate == null);
            if (alreadyActive) return BadRequest("Player is already an active member of this team.");

            var membership = new TeamPlayer
            {
                TeamId = teamId,
                PlayerId = playerId,
                JoinedDate = DateTime.UtcNow,
                LeftDate = null
            };
            _db.TeamPlayers.Add(membership);
            await _db.SaveChangesAsync();

            return Ok(new { message = $"{player.BroadcastName} added to {team.Name}." });
        }

        // DELETE: api/teams/5/players/12 — player ko team se hatao (history preserve rehti hai)
        [HttpDelete("{teamId}/players/{playerId}")]
        public async Task<IActionResult> RemovePlayerFromTeam(int teamId, int playerId)
        {
            var membership = await _db.TeamPlayers
                .Where(tp => tp.TeamId == teamId && tp.PlayerId == playerId && tp.LeftDate == null)
                .FirstOrDefaultAsync();

            if (membership == null) return NotFound("Active membership not found for this team/player.");

            membership.LeftDate = DateTime.UtcNow; // soft-remove — row delete nahi ho rahi
            await _db.SaveChangesAsync();

            return NoContent();
        }
    }
}