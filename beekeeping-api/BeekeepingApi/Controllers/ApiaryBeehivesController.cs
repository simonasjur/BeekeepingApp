using AutoMapper;
using BeekeepingApi.DTOs.ApiaryBeehiveDTOs;
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
    public class ApiaryBeehivesController : ControllerBase
    {
        private readonly BeekeepingContext _context;
        private readonly IMapper _mapper;

        public ApiaryBeehivesController(BeekeepingContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/ApiaryBeehives/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiaryBeehiveReadDTO>> GetOneApiaryBeehive(long id)
        {
            var apiaryBeehive = await _context.ApiaryBeehives.FindAsync(id);
            if (apiaryBeehive == null)
                return NotFound();

            var apiary = await _context.Apiaries.FindAsync(apiaryBeehive.ApiaryId);
            var currentUserId = long.Parse(User.Identity.Name);
            var farmWorker = await _context.FarmWorkers.FindAsync(currentUserId, apiary.FarmId);
            if (farmWorker == null)
            {
                return Forbid();
            }                

            return _mapper.Map<ApiaryBeehiveReadDTO>(apiaryBeehive);
        }

        //GET: api/apiaries/{apiaryId}/apiaryBeehives
        [HttpGet("/api/apiaries/{apiaryId}/apiaryBeehives")]
        public async Task<ActionResult<IEnumerable<ApiaryBeehiveReadForApiaryDTO>>> GetAllApiaryBeehives(long apiaryId)
        {
            var apiary = await _context.Apiaries.FindAsync(apiaryId);
            if (apiary == null)
            {
                return BadRequest();
            }

            var currentUserId = long.Parse(User.Identity.Name);
            var farmWorker = await _context.FarmWorkers.FindAsync(currentUserId, apiary.FarmId);
            if (farmWorker == null)
            {
                return Forbid();
            }

            var beehivesList = await _context.ApiaryBeehives.Where(ab => ab.ApiaryId == apiaryId &&
                                                                         ab.DepartDate == null).ToListAsync();
            foreach (ApiaryBeehive apiaryBeehive in beehivesList)
            {
                await _context.Entry(apiaryBeehive).Reference(ab => ab.Beehive).LoadAsync();
            }

            return _mapper.Map<IEnumerable<ApiaryBeehiveReadForApiaryDTO>>(beehivesList).ToList();
        }

        //GET: api/beehives/{beehiveId}/apiaryBeehives
        [HttpGet("/api/beehives/{beehiveId}/apiaryBeehives")]
        public async Task<ActionResult<IEnumerable<ApiaryBeehiveReadForBeehiveDTO>>> GetAllBeehiveApiaries(long beehiveId)
        {
            var beehive = await _context.Beehives.FindAsync(beehiveId);
            if (beehive == null)
            {
                return BadRequest();
            }

            var currentUserId = long.Parse(User.Identity.Name);
            var farmWorker = await _context.FarmWorkers.FindAsync(currentUserId, beehive.FarmId);
            if (farmWorker == null)
            {
                return Forbid();
            }

            var apiaryBeehivesList = await _context.ApiaryBeehives.Where(ab => ab.BeehiveId == beehiveId).ToListAsync();
            foreach (ApiaryBeehive apiaryBeehive in apiaryBeehivesList)
            {
                await _context.Entry(apiaryBeehive).Reference(ab => ab.Apiary).LoadAsync();
            }

            return _mapper.Map<IEnumerable<ApiaryBeehiveReadForBeehiveDTO>> (apiaryBeehivesList).ToList();
        }

        //POST: api/apiaryBeehives
        [HttpPost]
        public async Task<ActionResult<ApiaryBeehiveReadDTO>> CreateApiaryBeehive(ApiaryBeehiveCreateDTO apiaryBeehiveCreateDTO)
        {
            var beehiveApiaryDuplicate = _context.ApiaryBeehives.FirstOrDefault(ab =>
                ab.BeehiveId == apiaryBeehiveCreateDTO.BeehiveId &&
                ab.DepartDate == null);
            if (beehiveApiaryDuplicate != null)
            {
                return BadRequest();
            }

            var apiary = await _context.Apiaries.FindAsync(apiaryBeehiveCreateDTO.ApiaryId);
            if (apiary == null)
            {
                return BadRequest();
            }

            var beehive = await _context.Beehives.FindAsync(apiaryBeehiveCreateDTO.BeehiveId);
            if (beehive == null || apiary.FarmId != beehive.FarmId)
            {
                return BadRequest();
            }

            var currentUserId = long.Parse(User.Identity.Name);
            var farmWorker = await _context.FarmWorkers.FindAsync(currentUserId, apiary.FarmId);
            if (farmWorker == null)
            {
                return Forbid();
            }

            var apiaryBeehive = _mapper.Map<ApiaryBeehive>(apiaryBeehiveCreateDTO);
            _context.ApiaryBeehives.Add(apiaryBeehive);
            await _context.SaveChangesAsync();

            var apiaryBeehiveReadDTO = _mapper.Map<ApiaryBeehiveReadDTO>(apiaryBeehive);
            return CreatedAtAction(nameof(GetOneApiaryBeehive), new { id = apiaryBeehive.Id }, apiaryBeehiveReadDTO);
        }

        //PUT: api/apiaryBeehives/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateApiaryBeehive(long id, ApiaryBeehiveEditDTO apiaryBeehiveEditDTO)
        {
            if (id != apiaryBeehiveEditDTO.Id)
            {
                return BadRequest();
            }

            var apiaryBeehive = await _context.ApiaryBeehives.FindAsync(id);
            if (apiaryBeehive == null)
            {
                return BadRequest();
            }

            //If apiaryBeehive DepartDate have value, that value can't be null
            if (apiaryBeehive.DepartDate != null && apiaryBeehiveEditDTO.DepartDate == null)
            {
                return BadRequest();
            }

            var apiary = await _context.Beehives.FindAsync(apiaryBeehive.ApiaryId);
            var currentUserId = long.Parse(User.Identity.Name);
            var farmWorker = await _context.FarmWorkers.FindAsync(currentUserId, apiary.FarmId);
            if (farmWorker == null)
            {
                return Forbid();
            }

            _mapper.Map(apiaryBeehiveEditDTO, apiaryBeehive);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        //DELETE: api/apiaryBeehives/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiaryBeehiveReadDTO>> DeleteApiaryBeehive(long id)
        {
            var apiaryBeehive = await _context.ApiaryBeehives.FindAsync(id);
            if (apiaryBeehive == null)
            {
                return BadRequest();
            }

            var apiary = await _context.Apiaries.FindAsync(apiaryBeehive.ApiaryId);
            var currentUserId = long.Parse(User.Identity.Name);
            var farmWorker = await _context.FarmWorkers.FindAsync(currentUserId, apiary.FarmId);
            if (farmWorker == null)
            {
                return Forbid();
            }

            _context.ApiaryBeehives.Remove(apiaryBeehive);
            await _context.SaveChangesAsync();

            return _mapper.Map<ApiaryBeehiveReadDTO>(apiaryBeehive);
        }
    }
}
