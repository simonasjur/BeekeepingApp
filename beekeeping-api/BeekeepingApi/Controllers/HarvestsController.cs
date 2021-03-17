using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BeekeepingApi.Models;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using BeekeepingApi.DTOs.HarvestDTOs;

namespace BeekeepingApi.Controllers
{
    [Authorize]
    [Route("api/Farms/{farmId}/[controller]")]
    [ApiController]
    public class HarvestsController : ControllerBase
    {
        private readonly BeekeepingContext _context;
        private readonly IMapper _mapper;

        public HarvestsController(BeekeepingContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Farms/1/Harvests
        [HttpGet]
        public async Task<ActionResult<IEnumerable<HarvestReadDTO>>> GetHarvests(long farmId)
        {
            var currentUserId = long.Parse(User.Identity.Name);
            var farm = await _context.Farms.FindAsync(farmId);

            if (farm == null)
                return NotFound();

            var farmWorker = await _context.FarmWorkers.FindAsync(currentUserId, farmId);

            if (farmWorker == null)
                return Forbid();    

            var harvestList = await _context.Harvest.Where(l => l.FarmId == farmId).ToListAsync();

            return _mapper.Map<IEnumerable<HarvestReadDTO>>(harvestList).ToList();
        }

        // GET: api/Farms/1/Harvests/1
        [Authorize]
        [HttpGet("{harvestId}")]
        public async Task<ActionResult<HarvestReadDTO>> GetHarvest(long farmId, long harvestId)
        {
            var currentUserId = long.Parse(User.Identity.Name);

            var farmWorker = await _context.FarmWorkers.FindAsync(currentUserId, farmId);

            if (farmWorker == null)
                return Forbid();

            var harvest = await _context.Harvest.FindAsync(harvestId);

            return _mapper.Map<HarvestReadDTO>(harvest);
        }

        // PUT: api/Harvests/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutHarvest(long id, Harvest harvest)
        {
            if (id != harvest.Id)
            {
                return BadRequest();
            }

            _context.Entry(harvest).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HarvestExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Harvests
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Harvest>> PostHarvest(Harvest harvest)
        {
            _context.Harvest.Add(harvest);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetHarvest", new { id = harvest.Id }, harvest);
        }

        // DELETE: api/Harvests/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Harvest>> DeleteHarvest(long id)
        {
            var harvest = await _context.Harvest.FindAsync(id);
            if (harvest == null)
            {
                return NotFound();
            }

            _context.Harvest.Remove(harvest);
            await _context.SaveChangesAsync();

            return harvest;
        }

        private bool HarvestExists(long id)
        {
            return _context.Harvest.Any(e => e.Id == id);
        }
    }
}
