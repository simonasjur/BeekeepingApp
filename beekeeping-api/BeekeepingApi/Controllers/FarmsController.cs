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
using BeekeepingApi.DTOs.FarmDTOs;

namespace BeekeepingApi.Controllers
{
    [Authorize]
    [Route("api/Users/{userId}/[controller]")]
    [ApiController]
    public class FarmsController : ControllerBase
    {
        private readonly BeekeepingContext _context;
        private readonly IMapper _mapper;

        public FarmsController(BeekeepingContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Users/1/Farms
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FarmReadDTO>>> GetFarms(long userId)
        {
            var currentUserId = long.Parse(User.Identity.Name);
            if (userId != currentUserId)
                return Forbid();

            var user = await _context.Users.FindAsync(userId);

            var farmWorkersList = await _context.FarmWorkers.Where(l => l.UserId == userId).ToListAsync();
            var farmList = new List<Farm>();
            foreach (var farmWorker in farmWorkersList)
            {
                var farm = await _context.Farms.FindAsync(farmWorker.FarmId);
                farmList.Add(farm);
            }

            return _mapper.Map<IEnumerable<FarmReadDTO>>(farmList).ToList();
        }

        // GET: api/Users/1/Farms/1
        [HttpGet("{farmId}")]
        public async Task<ActionResult<FarmReadDTO>> GetFarm(long userId, long farmId)
        {
            var currentUserId = long.Parse(User.Identity.Name);
            if (userId != currentUserId)
                return Forbid();

            var farm = await _context.Farms.FindAsync(farmId);
            var farmWorker = await _context.FarmWorkers.FindAsync(userId, farmId);

            if (farm == null)
                return NotFound();

            if (farmWorker == null)
                return Forbid();

            return _mapper.Map<FarmReadDTO>(farm);
        }


        // POST: api/Users/5/Farms
        [HttpPost]
        public async Task<ActionResult<FarmReadDTO>> PostFarm(long userId, FarmCreateDTO farmCreateDTO)
        {
            var user = await _context.Users.FindAsync(userId);
            var currentUserId = long.Parse(User.Identity.Name);

            if (user == null || user.Id != currentUserId)
                return Forbid();

            var farm = _mapper.Map<Farm>(farmCreateDTO);
            _context.Farms.Add(farm);
            await _context.SaveChangesAsync();

            var farmWorker = new FarmWorker
            {
                Role = WorkerRole.Owner,
                FarmId = farm.Id,
                UserId = user.Id
            };
            _context.FarmWorkers.Add(farmWorker);
            await _context.SaveChangesAsync();

            var farmReadDTO = _mapper.Map<FarmReadDTO>(farm);

            return CreatedAtAction("GetFarm", "Farms", new { userId = user.Id, farmId = farm.Id }, farmReadDTO);
        }

        // PUT: api/Users/5/Farms/1
        [HttpPut("{farmId}")]
        public async Task<IActionResult> PutFarm(long userId, long farmId, FarmEditDTO farmEditDTO)
        {
            var currentUserId = long.Parse(User.Identity.Name);
            if (userId != currentUserId)
                return Forbid();

            if (farmId != farmEditDTO.Id)
                return BadRequest();

            var farm = await _context.Farms.FindAsync(farmId);
            if (farm == null)
                return NotFound();
            var farmWorker = await _context.FarmWorkers.FindAsync(userId, farmId);
            if (farmWorker == null)
                return Forbid();

            _mapper.Map(farmEditDTO, farm);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Users/5/Farms/5
        [HttpDelete("{farmId}")]
        public async Task<ActionResult<FarmReadDTO>> DeleteFarm(long userId, long farmId)
        {
            var currentUserId = long.Parse(User.Identity.Name);
            if (userId != currentUserId)
                return Forbid();

            var farm = await _context.Farms.FindAsync(farmId);
            var farmWorker = await _context.FarmWorkers.FindAsync(userId, farmId);

            if (farm == null)
                return NotFound();

            if (farmWorker == null)
                return Forbid();

            _context.Farms.Remove(farm);
            await _context.SaveChangesAsync();

            return _mapper.Map<FarmReadDTO>(farm);
        }
    }
}
