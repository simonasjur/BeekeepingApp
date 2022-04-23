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
using Microsoft.AspNet.OData;

namespace BeekeepingApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
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
        [HttpGet("/api/Farms/{farmId}/Apiaries")]
        [EnableQuery()]
        public async Task<ActionResult<IEnumerable<ApiaryReadDTO>>> GetFarmApiaries(long farmId)
        {           
            var farm = await _context.Farms.FindAsync(farmId);
            if (farm == null)
                return NotFound();

            var currentUserId = long.Parse(User.Identity.Name);
            var farmWorker = await _context.FarmWorkers.FindAsync(currentUserId, farmId);
            if (farmWorker == null)
                return Forbid();

            var apiariesList = await _context.Apiaries.Where(l => l.FarmId == farmId).ToListAsync();

            return _mapper.Map<IEnumerable<ApiaryReadDTO>>(apiariesList).ToList();
        }

        // GET: api/Apiaries/1
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiaryReadDTO>> GetApiary(long id)
        {
            var apiary = await _context.Apiaries.FindAsync(id);
            if (apiary == null)
                return NotFound();

            var currentUserId = long.Parse(User.Identity.Name);
            var farmWorker = await _context.FarmWorkers.FindAsync(currentUserId, apiary.FarmId);
            if (farmWorker == null)
                return Forbid();

            return _mapper.Map<ApiaryReadDTO>(apiary);
        }

        // POST: api/Apiaries
        [HttpPost]
        public async Task<ActionResult<ApiaryReadDTO>> CreateApiary(ApiaryCreateDTO apiaryCreateDTO)
        {
            var farm = await _context.Farms.FindAsync(apiaryCreateDTO.FarmId);
            if (farm == null)
                return BadRequest();

            var currentUserId = long.Parse(User.Identity.Name);
            var farmWorker = await _context.FarmWorkers.FindAsync(currentUserId, farm.Id);
            if (farmWorker == null || farmWorker.Role != WorkerRole.Owner)
                return Forbid();

            var apiary = _mapper.Map<Apiary>(apiaryCreateDTO);
            _context.Apiaries.Add(apiary);
            await _context.SaveChangesAsync();

            var apiaryReadDTO = _mapper.Map<ApiaryReadDTO>(apiary);

            return CreatedAtAction("GetApiary", "Apiaries", new { id = apiary.Id }, apiaryReadDTO);
        }

        // PUT: api/Apiaries/1
        [HttpPut("{id}")]
        public async Task<IActionResult> EditApiary(long id, ApiaryEditDTO apiaryEditDTO)
        {
            if (id != apiaryEditDTO.Id)
                return BadRequest();

            var apiary = await _context.Apiaries.FindAsync(id);
            if (apiary == null)
                return NotFound();
            var farm = await _context.Farms.FindAsync(apiary.FarmId);
            if (farm == null)
                return NotFound();

            var currentUserId = long.Parse(User.Identity.Name);
            var farmWorker = await _context.FarmWorkers.FindAsync(currentUserId, farm.Id);
            if (farmWorker == null || farmWorker.Role != WorkerRole.Owner)
                return Forbid();

            _mapper.Map(apiaryEditDTO, apiary);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Apiaries/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiaryReadDTO>> DeleteApiary(long id)
        {
            var apiary = await _context.Apiaries.FindAsync(id);
            if (apiary == null)
                return NotFound();
            var farm = await _context.Farms.FindAsync(apiary.FarmId);
            if (farm == null)
                return NotFound();

            var currentUserId = long.Parse(User.Identity.Name);
            var farmWorker = await _context.FarmWorkers.FindAsync(currentUserId, farm.Id);
            if (farmWorker == null || farmWorker.Role != WorkerRole.Owner)
                return Forbid();

            await _context.Entry(apiary).Collection(a => a.Harvests).LoadAsync();

            _context.Apiaries.Remove(apiary);
            await _context.SaveChangesAsync();

            return _mapper.Map<ApiaryReadDTO>(apiary);
        }
    }
}
