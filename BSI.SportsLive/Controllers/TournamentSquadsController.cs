using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BSI.SportsLive.Models;
using BSI.SportsLive.DTOs;
using AutoMapper;

namespace BSI.SportsLive.Controllers
{
    [ApiController]
    [Route("api/tournaments/{tournamentId}/teams/{teamId}/[controller]")]
    public class SquadController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly IMapper _mapper;

        public SquadController(AppDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetSquad(int tournamentId, int teamId)
        {
            var list = await _db.TeamSquads
                .Where(s => s.TournamentId == tournamentId && s.TeamId == teamId)
                .Include(s => s.Player)
                .ToListAsync();
            var dto = list.Select(s => new TeamSquadDto
            {
                TournamentId = s.TournamentId,
                TeamId = s.TeamId,
                PlayerId = s.PlayerId,
                PlayerName = s.Player?.FullName ?? string.Empty,
                IsPlayingXI = s.IsPlayingXI,
                IsCaptain = s.IsCaptain,
                BroadcastNameSnapshot = s.BroadcastNameSnapshot,
                ShirtNumberSnapshot = s.ShirtNumberSnapshot
            });
            return Ok(dto);
        }

        [HttpPost]
        public async Task<IActionResult> AddPlayer(int tournamentId, int teamId, [FromBody] TeamSquadCreateDto create)
        {
            // validate existence
            var tournament = await _db.Tournaments.FindAsync(tournamentId);
            if (tournament == null) return NotFound("Tournament not found");
            var team = await _db.Teams.FindAsync(teamId);
            if (team == null) return NotFound("Team not found");
            var player = await _db.Players.FindAsync(create.PlayerId);
            if (player == null) return NotFound("Player not found");

            var existing = await _db.TeamSquads.FindAsync(tournamentId, teamId, create.PlayerId);
            if (existing != null) return Conflict("Player already in squad");

            var squad = new TeamSquad
            {
                TournamentId = tournamentId,
                TeamId = teamId,
                PlayerId = create.PlayerId,
                IsPlayingXI = create.IsPlayingXI,
                IsCaptain = create.IsCaptain,
                BroadcastNameSnapshot = player.BroadcastName,
                ShirtNumberSnapshot = player.ShirtNumber
            };
            _db.TeamSquads.Add(squad);
            await _db.SaveChangesAsync();

            var dto = new TeamSquadDto
            {
                TournamentId = squad.TournamentId,
                TeamId = squad.TeamId,
                PlayerId = squad.PlayerId,
                PlayerName = player.FullName,
                IsPlayingXI = squad.IsPlayingXI,
                IsCaptain = squad.IsCaptain,
                BroadcastNameSnapshot = squad.BroadcastNameSnapshot,
                ShirtNumberSnapshot = squad.ShirtNumberSnapshot
            };
            return CreatedAtAction(nameof(GetSquad), new { tournamentId = tournamentId, teamId = teamId }, dto);
        }

        [HttpDelete("{playerId}")]
        public async Task<IActionResult> RemovePlayer(int tournamentId, int teamId, int playerId)
        {
            var squad = await _db.TeamSquads.FindAsync(tournamentId, teamId, playerId);
            if (squad == null) return NotFound();
            _db.TeamSquads.Remove(squad);
            await _db.SaveChangesAsync();
            return NoContent();
        }

        [HttpPost("playingxi")]
        public async Task<IActionResult> SetPlayingXI(int tournamentId, int teamId, [FromBody] PlayingXIUpdateDto dto)
        {
            // Validate players belong to squad
            var squadItems = await _db.TeamSquads.Where(s => s.TournamentId == tournamentId && s.TeamId == teamId).ToListAsync();
            var squadPlayerIds = squadItems.Select(s => s.PlayerId).ToHashSet();
            foreach (var pid in dto.PlayerIds)
            {
                if (!squadPlayerIds.Contains(pid)) return BadRequest($"Player {pid} is not in squad");
            }

            // Reset existing IsPlayingXI
            foreach (var sq in squadItems)
            {
                sq.IsPlayingXI = false;
            }

            // Set selected XI
            foreach (var pid in dto.PlayerIds)
            {
                var sq = squadItems.First(s => s.PlayerId == pid);
                sq.IsPlayingXI = true;
            }

            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}
