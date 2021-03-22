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
using Microsoft.AspNet.OData;

namespace BeekeepingApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
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
        [HttpGet("/api/Farms/{farmId}/Harvests")]
        [EnableQuery()]
        public async Task<ActionResult<IEnumerable<HarvestReadDTO>>> GetFarmHarvests(long farmId)
        {
            var farm = await _context.Farms.FindAsync(farmId);
            if (farm == null)
                return NotFound();

            var currentUserId = long.Parse(User.Identity.Name);
            var farmWorker = await _context.FarmWorkers.FindAsync(currentUserId, farmId);
            if (farmWorker == null)
                return Forbid();

            var harvestList = await _context.Harvests.Where(l => l.FarmId == farmId).ToListAsync();

            return _mapper.Map<IEnumerable<HarvestReadDTO>>(harvestList).ToList();
        }

        // GET: api/Apiaries/1/Harvests
        [HttpGet("/api/Apiaries/{apiaryId}/Harvests")]
        [EnableQuery()]
        public async Task<ActionResult<IEnumerable<HarvestReadDTO>>> GetApiaryHarvests(long apiaryId)
        {
            var currentUserId = long.Parse(User.Identity.Name);
            var apiary = await _context.Apiaries.FindAsync(apiaryId);
            if (apiary == null)
                return NotFound();
            var farm = await _context.Farms.FindAsync(apiary.FarmId);
            if (farm == null)
                return NotFound();

            var farmWorker = await _context.FarmWorkers.FindAsync(currentUserId, farm.Id);
            if (farmWorker == null)
                return Forbid();

            var harvestList = await _context.Harvests.Where(l => l.ApiaryId == apiaryId).ToListAsync();

            return _mapper.Map<IEnumerable<HarvestReadDTO>>(harvestList).ToList();
        }

        // GET: api/Harvests/1
        [HttpGet("{id}")]
        public async Task<ActionResult<HarvestReadDTO>> GetHarvest(long id)
        {            
            var harvest = await _context.Harvests.FindAsync(id);
            if (harvest == null)
                return NotFound();

            var farm = await _context.Farms.FindAsync(harvest.FarmId);

            var currentUserId = long.Parse(User.Identity.Name);
            var farmWorker = await _context.FarmWorkers.FindAsync(currentUserId, farm.Id);
            if (farmWorker == null)
                return Forbid();

            return _mapper.Map<HarvestReadDTO>(harvest);
        }

        // POST: api/Harvests
        [HttpPost]
        public async Task<ActionResult<HarvestReadDTO>> CreateHarvest(HarvestCreateDTO harvestCreateDTO)
        {
            var farm = await _context.Farms.FindAsync(harvestCreateDTO.FarmId);
            if (farm == null)
                return BadRequest();

            if (harvestCreateDTO.ApiaryId != null)
            {
                var apiary = await _context.Apiaries.FindAsync(harvestCreateDTO.ApiaryId);
                if (apiary == null || apiary.FarmId != farm.Id)
                    return BadRequest();
            }

            var currentUserId = long.Parse(User.Identity.Name);
            var farmWorker = await _context.FarmWorkers.FindAsync(currentUserId, farm.Id);
            if (farmWorker == null)
                return Forbid();

            var harvest = _mapper.Map<Harvest>(harvestCreateDTO);
            _context.Harvests.Add(harvest);
            await _context.SaveChangesAsync();

            var harvestReadDTO = _mapper.Map<HarvestReadDTO>(harvest);

            return CreatedAtAction("GetHarvest", "Harvests", new { id = harvest.Id }, harvestReadDTO);
        }

        // PUT: api/Harvests/1
        [HttpPut("{id}")]
        public async Task<IActionResult> EditHarvest(long id, HarvestEditDTO harvestEditDTO)
        {
            if (id != harvestEditDTO.Id)
                return BadRequest();

            var harvest = await _context.Harvests.FindAsync(id);
            if (harvest == null)
                return NotFound();
            var farm = await _context.Farms.FindAsync(harvest.FarmId);
            if (farm == null)
                return NotFound();

            var currentUserId = long.Parse(User.Identity.Name);
            var farmWorker = await _context.FarmWorkers.FindAsync(currentUserId, farm.Id);
            if (farmWorker == null)
                return Forbid();

            _mapper.Map(harvestEditDTO, harvest);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Harvests/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<HarvestReadDTO>> DeleteHarvest(long id)
        {
            var harvest = await _context.Harvests.FindAsync(id);
            if (harvest == null)
                return NotFound();
            var farm = await _context.Farms.FindAsync(harvest.FarmId);
            if (farm == null)
                return NotFound();

            var currentUserId = long.Parse(User.Identity.Name);
            var farmWorker = await _context.FarmWorkers.FindAsync(currentUserId, farm.Id);
            if (farmWorker == null)
                return Forbid();

            _context.Harvests.Remove(harvest);
            await _context.SaveChangesAsync();

            return _mapper.Map<HarvestReadDTO>(harvest);
        }
    }
}
