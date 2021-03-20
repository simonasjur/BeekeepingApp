using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BeekeepingApi.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using BeekeepingApi.DTOs.ApiaryDTOs;

namespace BeekeepingApi.Controllers
{
    [Authorize]
    [Route("api/Farms/{farmId}/[controller]")]
    [ApiController]
    public class ApiariesController : ControllerBase
    {
        private readonly BeekeepingContext _context;
        private readonly IMapper _mapper;

        public ApiariesController(BeekeepingContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Farms/1/Apiaries
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ApiaryReadDTO>>> GetApiaries(long farmId)
        {
            var currentUserId = long.Parse(User.Identity.Name);
            var farm = await _context.Farms.FindAsync(farmId);

            if (farm == null)
                return NotFound();

            var farmWorker = await _context.FarmWorkers.FindAsync(currentUserId, farmId);

            if (farmWorker == null)
                return Forbid();

            var harvestList = await _context.Apiaries.Where(l => l.FarmId == farmId).ToListAsync();

            return _mapper.Map<IEnumerable<ApiaryReadDTO>>(harvestList).ToList();
        }

        // GET: api/Farms/1/Apiaries/1
        [HttpGet("{apiaryId}")]
        public async Task<ActionResult<ApiaryReadDTO>> GetApiary(long farmId, long apiaryId)
        {
            var currentUserId = long.Parse(User.Identity.Name);
            var farmWorker = await _context.FarmWorkers.FindAsync(currentUserId, farmId);
            if (farmWorker == null)
                return Forbid();

            var apiary = await _context.Apiaries.FindAsync(apiaryId);
            if (apiary == null)
                return NotFound();

            return _mapper.Map<ApiaryReadDTO>(apiary);
        }

        // POST: api/Farms/5/Apiaries
        [HttpPost]
        public async Task<ActionResult<ApiaryReadDTO>> PostApiary(long farmId, ApiaryCreateDTO apiaryCreateDTO)
        {
            if (farmId != apiaryCreateDTO.FarmId)
                return BadRequest();

            var farm = await _context.Farms.FindAsync(farmId);
            if (farm == null)
                return BadRequest();

            var currentUserId = long.Parse(User.Identity.Name);
            var farmWorker = await _context.FarmWorkers.FindAsync(currentUserId, farmId);

            if (farmWorker == null)
                return Forbid();

            var apiary = _mapper.Map<Apiary>(apiaryCreateDTO);
            _context.Apiaries.Add(apiary);
            await _context.SaveChangesAsync();

            var apiaryReadDTO = _mapper.Map<ApiaryReadDTO>(apiary);

            return CreatedAtAction("GetApiary", "Apiaries", new { farmId = apiary.FarmId, apiaryId = apiary.Id }, apiaryReadDTO);
        }

        // PUT: api/Farms/5/Apiaries/1
        [HttpPut("{apiaryId}")]
        public async Task<IActionResult> PutApiary(long farmId, long apiaryId, ApiaryEditDTO apiaryEditDTO)
        {
            if (apiaryId != apiaryEditDTO.Id)
                return BadRequest();

            var farm = await _context.Farms.FindAsync(farmId);
            if (farm == null)
                return NotFound();
            var apiary = await _context.Apiaries.FindAsync(apiaryId);
            if (apiary == null)
                return NotFound();

            var currentUserId = long.Parse(User.Identity.Name);
            var farmWorker = await _context.FarmWorkers.FindAsync(currentUserId, farmId);
            if (farmWorker == null)
                return Forbid();

            _mapper.Map(apiaryEditDTO, apiary);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Farms/5/Apiaries/5
        [HttpDelete("{harvestId}")]
        public async Task<ActionResult<ApiaryReadDTO>> DeleteApiary(long farmId, long apiaryId)
        {
            var farm = await _context.Farms.FindAsync(farmId);
            if (farm == null)
                return NotFound();
            var apiary = await _context.Apiaries.FindAsync(apiaryId);
            if (apiary == null)
                return NotFound();

            var currentUserId = long.Parse(User.Identity.Name);
            var farmWorker = await _context.FarmWorkers.FindAsync(currentUserId, farmId);
            if (farmWorker == null)
                return Forbid();

            _context.Apiaries.Remove(apiary);
            await _context.SaveChangesAsync();

            return _mapper.Map<ApiaryReadDTO>(apiary);
        }
    }
}
