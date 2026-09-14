using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BSI.SportsLive.Models;
using BSI.SportsLive.DTOs;
using AutoMapper;

namespace BSI.SportsLive.Controllers
{
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
            var teams = await _db.Teams.Include(t => t.Players).ToListAsync();
            var dto = _mapper.Map<IEnumerable<TeamDto>>(teams);
            return Ok(dto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var team = await _db.Teams.Include(t => t.Players).FirstOrDefaultAsync(t => t.Id == id);
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
    }
}
