using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using BSI.SportsLive.Models;
using BSI.SportsLive.DTOs;
using AutoMapper;

namespace BSI.SportsLive.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class PlayersController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly IMapper _mapper;

        public PlayersController(AppDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var players = await _db.Players
                .Include(p => p.TeamPlayers)
                .ThenInclude(tp => tp.Team)
                .ToListAsync();
            var dto = _mapper.Map<IEnumerable<PlayerDto>>(players);
            return Ok(dto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var player = await _db.Players
                .Include(p => p.TeamPlayers)
                .ThenInclude(tp => tp.Team)
                .FirstOrDefaultAsync(p => p.Id == id);
            if (player == null) return NotFound();
            var dto = _mapper.Map<PlayerDto>(player);
            return Ok(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PlayerCreateDto playerCreate)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var player = _mapper.Map<Player>(playerCreate);
            if (string.IsNullOrWhiteSpace(player.BroadcastName))
            {
                player.BroadcastName = GenerateBroadcastName(player.FullName);
            }
            _db.Players.Add(player);
            await _db.SaveChangesAsync();
            var dto = _mapper.Map<PlayerDto>(player);
            return CreatedAtAction(nameof(Get), new { id = player.Id }, dto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] PlayerUpdateDto updated)
        {
            if (id != updated.Id) return BadRequest();
            var player = await _db.Players.FindAsync(id);
            if (player == null) return NotFound();
            _mapper.Map(updated, player);
            // respect explicit BroadcastName, otherwise regenerate
            player.BroadcastName = string.IsNullOrWhiteSpace(updated.BroadcastName)
                ? GenerateBroadcastName(updated.FullName)
                : updated.BroadcastName!;
            await _db.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var player = await _db.Players.FindAsync(id);
            if (player == null) return NotFound();
            _db.Players.Remove(player);
            await _db.SaveChangesAsync();
            return NoContent();
        }

        private static string GenerateBroadcastName(string fullName)
        {
            if (string.IsNullOrWhiteSpace(fullName)) return string.Empty;
            var parts = fullName.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 1) return parts[0];
            // Format: FirstName + ' ' + FirstInitial.
            var first = parts[0];
            var lastInitial = parts.Length > 1 ? parts[1][0].ToString().ToUpper() + "." : string.Empty;
            return $"{first} {lastInitial}";
        }
    }
}