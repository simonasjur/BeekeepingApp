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
        public async Task<ActionResult<IEnumerable<HarvestReadDTO>>> GetFarmHarvests(long farmId)
        {
            var currentUserId = long.Parse(User.Identity.Name);
            var farm = await _context.Farms.FindAsync(farmId);

            if (farm == null)
                return NotFound();

            var farmWorker = await _context.FarmWorkers.FindAsync(currentUserId, farmId);

            if (farmWorker == null)
                return Forbid();

            var harvestList = await _context.Harvests.Where(l => l.FarmId == farmId).ToListAsync();

            return _mapper.Map<IEnumerable<HarvestReadDTO>>(harvestList).ToList();
        }

        // GET: api/Farms/1/Harvests/1
        [HttpGet("{harvestId}")]
        public async Task<ActionResult<HarvestReadDTO>> GetFarmHarvest(long farmId, long harvestId)
        {
            var currentUserId = long.Parse(User.Identity.Name);
            var farmWorker = await _context.FarmWorkers.FindAsync(currentUserId, farmId);
            if (farmWorker == null)
                return Forbid();

            var harvest = await _context.Harvests.FindAsync(harvestId);
            if (harvest == null)
                return NotFound();

            return _mapper.Map<HarvestReadDTO>(harvest);
        }

        // POST: api/Farms/5/Harvests
        [HttpPost]
        public async Task<ActionResult<HarvestReadDTO>> PostFarmHarvest(long farmId, HarvestCreateDTO harvestCreateDTO)
        {
            if (farmId != harvestCreateDTO.FarmId)
                return BadRequest();

            var farm = await _context.Farms.FindAsync(farmId);
            if (farm == null)
                return BadRequest();

            var currentUserId = long.Parse(User.Identity.Name);
            var farmWorker = await _context.FarmWorkers.FindAsync(currentUserId, farmId);

            if (farmWorker == null)
                return Forbid();

            var harvest = _mapper.Map<Harvest>(harvestCreateDTO);
            _context.Harvests.Add(harvest);
            await _context.SaveChangesAsync();

            var harvestReadDTO = _mapper.Map<HarvestReadDTO>(harvest);

            return CreatedAtAction("GetFarmHarvest", "Harvests", new { farmId = harvest.FarmId, harvestId = harvest.Id }, harvestReadDTO);
        }

        // PUT: api/Farms/5/Harvests/1
        [HttpPut("{harvestId}")]
        public async Task<IActionResult> PutFarmHarvest(long farmId, long harvestId, HarvestEditDTO harvestEditDTO)
        {
            if (harvestId != harvestEditDTO.Id)
                return BadRequest();

            var farm = await _context.Farms.FindAsync(farmId);
            if (farm == null)
                return NotFound();
            var harvest = await _context.Harvests.FindAsync(harvestId);
            if (harvest == null)
                return NotFound();

            var currentUserId = long.Parse(User.Identity.Name);
            var farmWorker = await _context.FarmWorkers.FindAsync(currentUserId, farmId);
            if (farmWorker == null)
                return Forbid();

            _mapper.Map(harvestEditDTO, harvest);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Farms/5/Harvests/5
        [HttpDelete("{harvestId}")]
        public async Task<ActionResult<HarvestReadDTO>> DeleteFarmHarvest(long farmId, long harvestId)
        {
            var farm = await _context.Farms.FindAsync(farmId);
            if (farm == null)
                return NotFound();
            var harvest = await _context.Harvests.FindAsync(harvestId);
            if (harvest == null)
                return NotFound();

            var currentUserId = long.Parse(User.Identity.Name);
            var farmWorker = await _context.FarmWorkers.FindAsync(currentUserId, farmId);
            if (farmWorker == null)
                return Forbid();

            _context.Harvests.Remove(harvest);
            await _context.SaveChangesAsync();

            return _mapper.Map<HarvestReadDTO>(harvest);
        }

        // GET: api/Apiaries/1/Harvests
        [HttpGet("/api/Apiaries/{apiaryId}/Harvests")]
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

        // GET: api/Apiaries/1/Harvests/1
        [HttpGet("/api/Apiaries/{apiaryId}/Harvests/{harvestId}")]
        public async Task<ActionResult<HarvestReadDTO>> GetApiaryHarvest(long apiaryId, long harvestId)
        {
            var currentUserId = long.Parse(User.Identity.Name);
            var apiary = await _context.Apiaries.FindAsync(apiaryId);
            if (apiary == null)
                return NotFound();

            var farmWorker = await _context.FarmWorkers.FindAsync(currentUserId, apiary.FarmId);
            if (farmWorker == null)
                return Forbid();

            var harvest = await _context.Harvests.FindAsync(harvestId);
            if (harvest == null || harvest.ApiaryId != apiaryId)
                return NotFound();

            return _mapper.Map<HarvestReadDTO>(harvest);
        }

        // POST: api/Apiaries/5/Harvests
        [HttpPost("/api/Apiaries/{apiaryId}/Harvests/")]
        public async Task<ActionResult<HarvestReadDTO>> PostApiaryHarvest(long apiaryId, HarvestCreateDTO harvestCreateDTO)
        {
            if (apiaryId != harvestCreateDTO.ApiaryId)
                return BadRequest();

            var apiary = await _context.Apiaries.FindAsync(apiaryId);
            if (apiary == null)
                return NotFound();
            if (apiary.FarmId != harvestCreateDTO.FarmId)
                return BadRequest();

            var farm = await _context.Farms.FindAsync(apiary.FarmId);
            if (farm == null)
                return BadRequest();

            var currentUserId = long.Parse(User.Identity.Name);
            var farmWorker = await _context.FarmWorkers.FindAsync(currentUserId, farm.Id);

            if (farmWorker == null)
                return Forbid();

            var harvest = _mapper.Map<Harvest>(harvestCreateDTO);
            _context.Harvests.Add(harvest);
            await _context.SaveChangesAsync();

            var harvestReadDTO = _mapper.Map<HarvestReadDTO>(harvest);

            return CreatedAtAction("GetApiaryHarvest", new { apiaryId = harvest.ApiaryId, harvestId = harvest.Id }, harvestReadDTO);
        }

        // PUT: api/Apiaries/5/Harvests/1
        [HttpPut("/api/Apiaries/{apiaryId}/Harvests/{harvestId}")]
        public async Task<IActionResult> PutApiarieHarvest(long apiaryId, long harvestId, HarvestEditDTO harvestEditDTO)
        {
            if (harvestId != harvestEditDTO.Id)
                return BadRequest();

            var apiary = await _context.Apiaries.FindAsync(apiaryId);
            if (apiary == null)
                return NotFound();
            var farm = await _context.Farms.FindAsync(apiary.FarmId);
            if (farm == null)
                return NotFound();
            var harvest = await _context.Harvests.FindAsync(harvestId);
            if (harvest == null || harvest.ApiaryId != apiaryId)
                return NotFound();

            var currentUserId = long.Parse(User.Identity.Name);
            var farmWorker = await _context.FarmWorkers.FindAsync(currentUserId, farm.Id);
            if (farmWorker == null)
                return Forbid();

            _mapper.Map(harvestEditDTO, harvest);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Apiaries/5/Harvests/5
        [HttpDelete("/api/Apiaries/{apiaryId}/Harvests/{harvestId}")]
        public async Task<ActionResult<HarvestReadDTO>> DeleteApiaryHarvest(long apiaryId, long harvestId)
        {
            var apiary = await _context.Apiaries.FindAsync(apiaryId);
            if (apiary == null)
                return NotFound();
            var farm = await _context.Farms.FindAsync(apiary.FarmId);
            if (farm == null)
                return NotFound();
            var harvest = await _context.Harvests.FindAsync(harvestId);
            if (harvest == null || harvest.ApiaryId != apiaryId)
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
