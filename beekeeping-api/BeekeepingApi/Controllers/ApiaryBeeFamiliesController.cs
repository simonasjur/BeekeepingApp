using AutoMapper;
using BeekeepingApi.DTOs.ApiaryBeeFamilyDTOs;
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
    public class ApiaryBeeFamiliesController : ControllerBase
    {
        private readonly BeekeepingContext _context;
        private readonly IMapper _mapper;

        public ApiaryBeeFamiliesController(BeekeepingContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/apiaryBeeFamilies/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiaryBeeFamilyReadDTO>> GetOneApiaryBeeFamily(long id)
        {
            var apiaryBeeFamily = await _context.ApiaryBeeFamilies.FindAsync(id);
            if (apiaryBeeFamily == null)
                return NotFound();

            var apiary = await _context.Apiaries.FindAsync(apiaryBeeFamily.ApiaryId);
            var currentUserId = long.Parse(User.Identity.Name);
            var farmWorker = await _context.FarmWorkers.FindAsync(currentUserId, apiary.FarmId);
            if (farmWorker == null)
            {
                return Forbid();
            }                

            return _mapper.Map<ApiaryBeeFamilyReadDTO>(apiaryBeeFamily);
        }

        //GET: api/apiaries/{apiaryId}/apiaryBeeFamilies
        [HttpGet("/api/apiaries/{apiaryId}/apiaryBeeFamilies")]
        public async Task<ActionResult<IEnumerable<ApiaryBeeFamilyReadForApiaryDTO>>> GetAllApiaryBeeFamilies(long apiaryId)
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

            var beeFamiliesList = await _context.ApiaryBeeFamilies.Where(ab => ab.ApiaryId == apiaryId &&
                                                                         ab.DepartDate == null).ToListAsync();
            foreach (ApiaryBeeFamily apiaryBeeFamily in beeFamiliesList)
            {
                await _context.Entry(apiaryBeeFamily).Reference(ab => ab.BeeFamily).LoadAsync();
            }

            return _mapper.Map<IEnumerable<ApiaryBeeFamilyReadForApiaryDTO>>(beeFamiliesList).ToList();
        }

        //GET: api/beeFamilies/{beeFamilyId}/apiaryBeeFamilies
        [HttpGet("/api/beeFamilies/{beeFamilyId}/apiaryBeeFamilies")]
        public async Task<ActionResult<IEnumerable<ApiaryBeeFamilyReadForBeeFamilyDTO>>> GetAllBeeFamilyApiaries(long beeFamilyId)
        {
            var beeFamily = await _context.BeeFamilies.FindAsync(beeFamilyId);
            if (beeFamily == null)
            {
                return BadRequest();
            }

            var currentUserId = long.Parse(User.Identity.Name);
            var farmWorker = await _context.FarmWorkers.FindAsync(currentUserId, beeFamily.FarmId);
            if (farmWorker == null)
            {
                return Forbid();
            }

            var apiaryBeeFamiliesList = await _context.ApiaryBeeFamilies.Where(ab => ab.BeeFamilyId == beeFamilyId).ToListAsync();
            foreach (ApiaryBeeFamily apiaryBeehive in apiaryBeeFamiliesList)
            {
                await _context.Entry(apiaryBeehive).Reference(ab => ab.Apiary).LoadAsync();
            }

            return _mapper.Map<IEnumerable<ApiaryBeeFamilyReadForBeeFamilyDTO>> (apiaryBeeFamiliesList).ToList();
        }

        //POST: api/apiaryBeeFamilies
        [HttpPost]
        public async Task<ActionResult<ApiaryBeeFamilyReadDTO>> CreateApiaryBeeFamily(ApiaryBeeFamilyCreateDTO apiaryBeeFamilyCreateDTO)
        {
            var beeFamilyApiaryDuplicate = _context.ApiaryBeeFamilies.FirstOrDefault(ab =>
                ab.BeeFamilyId == apiaryBeeFamilyCreateDTO.BeeFamilyId &&
                ab.DepartDate == null);
            if (beeFamilyApiaryDuplicate != null)
            {
                return BadRequest();
            }

            var apiary = await _context.Apiaries.FindAsync(apiaryBeeFamilyCreateDTO.ApiaryId);
            if (apiary == null)
            {
                return BadRequest();
            }

            var beeFamily = await _context.BeeFamilies.FindAsync(apiaryBeeFamilyCreateDTO.BeeFamilyId);
            if (beeFamily == null || apiary.FarmId != beeFamily.FarmId)
            {
                return BadRequest();
            }

            var currentUserId = long.Parse(User.Identity.Name);
            var farmWorker = await _context.FarmWorkers.FindAsync(currentUserId, apiary.FarmId);
            if (farmWorker == null)
            {
                return Forbid();
            }

            var apiaryBeehive = _mapper.Map<ApiaryBeeFamily>(apiaryBeeFamilyCreateDTO);
            _context.ApiaryBeeFamilies.Add(apiaryBeehive);
            await _context.SaveChangesAsync();

            var apiaryBeehiveReadDTO = _mapper.Map<ApiaryBeeFamilyReadDTO>(apiaryBeehive);
            return CreatedAtAction(nameof(GetOneApiaryBeeFamily), new { id = apiaryBeehive.Id }, apiaryBeehiveReadDTO);
        }

        //PUT: api/apiaryBeeFamilies/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateApiaryBeeFamily(long id, ApiaryBeeFamilyEditDTO apiaryBeeFamilyEditDTO)
        {
            if (id != apiaryBeeFamilyEditDTO.Id)
            {
                return BadRequest();
            }

            var apiaryBeeFamily = await _context.ApiaryBeeFamilies.FindAsync(id);
            if (apiaryBeeFamily == null)
            {
                return BadRequest();
            }

            //If apiaryBeeFamily DepartDate have value, that value can't be null
            if (apiaryBeeFamily.DepartDate != null && apiaryBeeFamilyEditDTO.DepartDate == null)
            {
                return BadRequest();
            }

            var apiary = await _context.BeeFamilies.FindAsync(apiaryBeeFamily.ApiaryId);
            var currentUserId = long.Parse(User.Identity.Name);
            var farmWorker = await _context.FarmWorkers.FindAsync(currentUserId, apiary.FarmId);
            if (farmWorker == null)
            {
                return Forbid();
            }

            _mapper.Map(apiaryBeeFamilyEditDTO, apiaryBeeFamily);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        //DELETE: api/apiaryBeeFamilies/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiaryBeeFamilyReadDTO>> DeleteApiaryBeeFamily(long id)
        {
            var apiaryBeeFamily = await _context.ApiaryBeeFamilies.FindAsync(id);
            if (apiaryBeeFamily == null)
            {
                return BadRequest();
            }

            var apiary = await _context.Apiaries.FindAsync(apiaryBeeFamily.ApiaryId);
            var currentUserId = long.Parse(User.Identity.Name);
            var farmWorker = await _context.FarmWorkers.FindAsync(currentUserId, apiary.FarmId);
            if (farmWorker == null)
            {
                return Forbid();
            }

            _context.ApiaryBeeFamilies.Remove(apiaryBeeFamily);
            await _context.SaveChangesAsync();

            return _mapper.Map<ApiaryBeeFamilyReadDTO>(apiaryBeeFamily);
        }
    }
}
