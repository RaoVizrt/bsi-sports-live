using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BSI.SportsLive.Models;
using BSI.SportsLive.DTOs;
using AutoMapper;

namespace BSI.SportsLive.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SchedulesController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly IMapper _mapper;

        public SchedulesController(AppDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var schedules = await _db.Schedules
                .Include(s => s.Tournament)
                .Include(s => s.TeamA)
                .Include(s => s.TeamB)
                .ToListAsync();
            var dto = _mapper.Map<IEnumerable<ScheduleDto>>(schedules);
            return Ok(dto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var schedule = await _db.Schedules
                .Include(s => s.Tournament)
                .Include(s => s.TeamA)
                .Include(s => s.TeamB)
                .FirstOrDefaultAsync(s => s.Id == id);
            if (schedule == null) return NotFound();
            var dto = _mapper.Map<ScheduleDto>(schedule);
            return Ok(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ScheduleCreateDto create)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            // basic validation: ensure teams exist
            var teamA = await _db.Teams.FindAsync(create.TeamAId);
            var teamB = await _db.Teams.FindAsync(create.TeamBId);
            if (teamA == null || teamB == null) return BadRequest("TeamAId or TeamBId invalid");

            var schedule = _mapper.Map<Schedule>(create);
            _db.Schedules.Add(schedule);
            await _db.SaveChangesAsync();
            var dto = _mapper.Map<ScheduleDto>(await _db.Schedules.Include(s => s.Tournament).Include(s => s.TeamA).Include(s => s.TeamB).FirstOrDefaultAsync(s => s.Id == schedule.Id));
            return CreatedAtAction(nameof(Get), new { id = schedule.Id }, dto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ScheduleUpdateDto updated)
        {
            if (id != updated.Id) return BadRequest();
            var schedule = await _db.Schedules.FindAsync(id);
            if (schedule == null) return NotFound();
            _mapper.Map(updated, schedule);
            await _db.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var schedule = await _db.Schedules.FindAsync(id);
            if (schedule == null) return NotFound();
            _db.Schedules.Remove(schedule);
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}
