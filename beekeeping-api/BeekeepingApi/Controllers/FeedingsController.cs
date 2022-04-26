using AutoMapper;
using BeekeepingApi.DTOs.FeedingDTOs;
using BeekeepingApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeekeepingApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class FeedingsController : ControllerBase
    {
        private readonly BeekeepingContext _context;
        private readonly IMapper _mapper;

        public FeedingsController(BeekeepingContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Beefamilies/1/Feedings
        [HttpGet("/api/Beefamilies/{beefamilyId}/Feedings")]
        public async Task<ActionResult<IEnumerable<FeedingReadDTO>>> GetBeefamilyFeedings(long beefamilyId)
        {
            var beefamily = await _context.BeeFamilies.FindAsync(beefamilyId);
            if (beefamily == null)
            {
                return NotFound();
            }

            var currentUserId = long.Parse(User.Identity.Name);
            var farmWorker = await _context.FarmWorkers.FindAsync(currentUserId, beefamily.FarmId);
            if (farmWorker == null)
            {
                return Forbid();
            }

            var feedings = await _context.Feedings.Where(f => f.BeeFamilyId == beefamilyId).ToListAsync();

            return _mapper.Map<IEnumerable<FeedingReadDTO>>(feedings).ToList();
        }

        // GET: api/Feedings/1
        [HttpGet("{id}")]
        public async Task<ActionResult<FeedingReadDTO>> GetFeeding(long id)
        {
            var feeding = await _context.Feedings.FindAsync(id);
            if (feeding == null)
            {
                return NotFound();
            }

            var beefamily = await _context.BeeFamilies.FindAsync(feeding.BeeFamilyId);
            var currentUserId = long.Parse(User.Identity.Name);
            var farmWorker = await _context.FarmWorkers.FindAsync(currentUserId, beefamily.FarmId);
            if (farmWorker == null)
            {
                return Forbid();
            }

            return _mapper.Map<FeedingReadDTO>(feeding);
        }

        // POST: api/Feedings
        [HttpPost]
        public async Task<ActionResult<FeedingReadDTO>> CreateFeeding(FeedingCreateDTO feedingCreateDTO)
        {
            var beefamily = await _context.BeeFamilies.FindAsync(feedingCreateDTO.BeeFamilyId);
            if (beefamily == null)
            {
                return BadRequest();
            }

            var food = await _context.Foods.FindAsync(feedingCreateDTO.FoodId);
            if (food == null || beefamily.FarmId != food.FarmId)
            {
                return BadRequest();
            }

            var currentUserId = long.Parse(User.Identity.Name);
            var farmWorker = await _context.FarmWorkers.FindAsync(currentUserId, beefamily.FarmId);
            if (farmWorker == null)
            {
                return Forbid();
            }


            var feeding = _mapper.Map<Feeding>(feedingCreateDTO);
            _context.Feedings.Add(feeding);
            await _context.SaveChangesAsync();

            var feedingReadDTO = _mapper.Map<FeedingReadDTO>(feeding);

            return CreatedAtAction("GetFeeding", "Feedings", new { id = feeding.Id }, feedingReadDTO);
        }

        // PUT: api/Feedings/1
        [HttpPut("{id}")]
        public async Task<IActionResult> EditFeeding(long id, FeedingEditDTO feedingEditDTO)
        {
            if (id != feedingEditDTO.Id)
            {
                return BadRequest();
            }

            var feeding = await _context.Feedings.FindAsync(id);
            if (feeding == null)
            {
                return NotFound();
            }

            var beefamily = await _context.BeeFamilies.FindAsync(feeding.BeeFamilyId);
            var currentUserId = long.Parse(User.Identity.Name);
            var farmWorker = await _context.FarmWorkers.FindAsync(currentUserId, beefamily.FarmId);
            if (farmWorker == null)
            {
                return Forbid();
            }

            _mapper.Map(feedingEditDTO, feeding);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Feedings/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<FeedingReadDTO>> DeleteFeeding(long id)
        {
            var feeding = await _context.Feedings.FindAsync(id);
            if (feeding == null)
            {
                return NotFound();
            }

            var beefamily = await _context.BeeFamilies.FindAsync(feeding.BeeFamilyId);
            var currentUserId = long.Parse(User.Identity.Name);
            var farmWorker = await _context.FarmWorkers.FindAsync(currentUserId, beefamily.FarmId);
            if (farmWorker == null)
            {
                return Forbid();
            }

            _context.Feedings.Remove(feeding);
            await _context.SaveChangesAsync();

            return _mapper.Map<FeedingReadDTO>(feeding);
        }
    }
}
