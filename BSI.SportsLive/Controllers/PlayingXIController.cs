using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BSI.SportsLive.Models;
using BSI.SportsLive.DTOs;

namespace BSI.SportsLive.Controllers
{
    [ApiController]
    [Route("api/matches/{matchId}/[controller]")]
    public class PlayingXIController : ControllerBase
    {
        private readonly AppDbContext _db;

        public PlayingXIController(AppDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(int matchId, [FromQuery] int? teamId)
        {
            var query = _db.MatchPlayingXIs.Where(x => x.MatchId == matchId).AsQueryable();
            if (teamId.HasValue) query = query.Where(x => x.TeamId == teamId.Value);
            var list = await query.Include(x => x.Player).Include(x => x.Team).OrderBy(x => x.TeamId).ThenBy(x => x.Position).ToListAsync();

            var grouped = list.GroupBy(x => x.TeamId).Select(g => new
            {
                TeamId = g.Key,
                TeamName = g.First().Team?.Name ?? string.Empty,
                Players = g.Select(p => new PlayingXIPlayerDto
                {
                    PlayerId = p.PlayerId,
                    PlayerName = p.Player?.FullName ?? string.Empty,
                    BroadcastName = p.BroadcastNameSnapshot,
                    ShirtNumber = p.ShirtNumberSnapshot,
                    Position = p.Position,
                    IsCaptain = p.IsCaptain
                }).OrderBy(p => p.Position).ToList()
            });

            return Ok(grouped);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrReplace(int matchId, [FromBody] PlayingXIUpsertDto dto)
        {
            var match = await _db.Matches.FindAsync(matchId);
            if (match == null) return NotFound("Match not found");
            var team = await _db.Teams.FindAsync(dto.TeamId);
            if (team == null) return BadRequest("Team not found");

            // Validate players
            var playerIds = dto.Players.Select(p => p.PlayerId).ToList();
            var players = await _db.Players.Where(p => playerIds.Contains(p.Id)).ToListAsync();
            if (players.Count != playerIds.Count) return BadRequest("One or more players not found");

            // Remove existing XI for match+team
            var existing = _db.MatchPlayingXIs.Where(x => x.MatchId == matchId && x.TeamId == dto.TeamId);
            _db.MatchPlayingXIs.RemoveRange(existing);

            // Insert new snapshot
            var inserts = new List<MatchPlayingXI>();
            foreach (var p in dto.Players)
            {
                var player = players.First(pl => pl.Id == p.PlayerId);
                inserts.Add(new MatchPlayingXI
                {
                    MatchId = matchId,
                    TeamId = dto.TeamId,
                    PlayerId = p.PlayerId,
                    Position = p.Position,
                    IsCaptain = p.IsCaptain,
                    BroadcastNameSnapshot = player.BroadcastName,
                    ShirtNumberSnapshot = player.ShirtNumber
                });
            }

            _db.MatchPlayingXIs.AddRange(inserts);
            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAll), new { matchId = matchId, teamId = dto.TeamId }, null);
        }

        [HttpDelete("{teamId}")]
        public async Task<IActionResult> Delete(int matchId, int teamId)
        {
            var items = _db.MatchPlayingXIs.Where(x => x.MatchId == matchId && x.TeamId == teamId);
            if (!await items.AnyAsync()) return NotFound();
            _db.MatchPlayingXIs.RemoveRange(items);
            await _db.SaveChangesAsync();
            return NoContent();
        }

        // Export playing XI in vMix-friendly JSON format for broadcast graphics
        [HttpGet("{teamId}/export/vmix")]
        public async Task<IActionResult> ExportForVmix(int matchId, int teamId)
        {
            var match = await _db.Matches
                .Include(m => m.Tournament)
                .FirstOrDefaultAsync(m => m.Id == matchId);
            if (match == null) return NotFound("Match not found");

            var team = await _db.Teams.FindAsync(teamId);
            if (team == null) return NotFound("Team not found");

            var xi = await _db.MatchPlayingXIs
                .Where(x => x.MatchId == matchId && x.TeamId == teamId)
                .Include(x => x.Player)
                .OrderBy(x => x.Position)
                .ToListAsync();

            if (!xi.Any()) return NotFound("Playing XI not found for this match/team");

            var payload = new
            {
                source = "BSI.SportsLive",
                generatedAt = DateTime.UtcNow,
                match = new
                {
                    id = match.Id,
                    tournament = match.Tournament?.Name ?? "Friendly Match", // Nayi field ke hisaab se
                    date = match.MatchDate,
                    team = new { id = team.Id, name = team.Name }
                },
                playingXI = xi.Select(p => new
                {
                    position = p.Position,
                    playerId = p.PlayerId,
                    name = p.Player?.FullName ?? string.Empty,
                    broadcastName = p.BroadcastNameSnapshot,
                    shirtNumber = p.ShirtNumberSnapshot,
                    isCaptain = p.IsCaptain
                }).ToList()
            };

            return Ok(payload);
        }
    }
}
