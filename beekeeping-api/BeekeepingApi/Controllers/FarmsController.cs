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
using Microsoft.AspNet.OData;

namespace BeekeepingApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
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
        [HttpGet("/api/Users/{userId}/Farms")]
        [EnableQuery()]
        public async Task<ActionResult<IEnumerable<FarmReadDTO>>> GetUserFarms(long userId)
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

        // GET: api/Farms/1
        [HttpGet("{id}")]
        public async Task<ActionResult<FarmReadDTO>> GetFarm(long id)
        {
            var farm = await _context.Farms.FindAsync(id);
            if (farm == null)
                return NotFound();

            var currentUserId = long.Parse(User.Identity.Name);
            var farmWorker = await _context.FarmWorkers.FindAsync(currentUserId, id);

            if (farmWorker == null)
                return Forbid();

            return _mapper.Map<FarmReadDTO>(farm);
        }


        // POST: api/Farms
        [HttpPost]
        public async Task<ActionResult<FarmReadDTO>> CreateFarm(FarmCreateDTO farmCreateDTO)
        {
            var currentUserId = long.Parse(User.Identity.Name);
            var farm = _mapper.Map<Farm>(farmCreateDTO);

            _context.Farms.Add(farm);
            await _context.SaveChangesAsync();

            var farmWorkersList = await _context.FarmWorkers.Where(l => l.UserId == currentUserId).ToListAsync();
            if (!farmWorkersList.Any())
            {
                var user = await _context.Users.FindAsync(currentUserId);
                user.DefaultFarmId = farm.Id;
                _context.Entry(user).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }

            var farmWorker = new FarmWorker
            {
                Role = WorkerRole.Owner,
                FarmId = farm.Id,
                UserId = currentUserId,
                Permissions = "111111111111111111111111111111"
            };
            _context.FarmWorkers.Add(farmWorker);
            await _context.SaveChangesAsync();

            var farmReadDTO = _mapper.Map<FarmReadDTO>(farm);

            return CreatedAtAction("GetFarm", "Farms", new { id = farm.Id }, farmReadDTO);
        }

        // PUT: api/Farms/1
        [HttpPut("{id}")]
        public async Task<IActionResult> EditFarm(long id, FarmEditDTO farmEditDTO)
        {
            if (id != farmEditDTO.Id)
                return BadRequest();

            var farm = await _context.Farms.FindAsync(id);
            if (farm == null)
                return NotFound();
            var currentUserId = long.Parse(User.Identity.Name);
            var farmWorker = await _context.FarmWorkers.FindAsync(currentUserId, id);
            if (farmWorker == null || farmWorker.Role != WorkerRole.Owner)
                return Forbid();

            _mapper.Map(farmEditDTO, farm);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Farms/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<FarmReadDTO>> DeleteFarm(long id)
        {
            var farm = await _context.Farms.FindAsync(id);
            if (farm == null)
                return NotFound();
            var currentUserId = long.Parse(User.Identity.Name);
            var farmWorker = await _context.FarmWorkers.FindAsync(currentUserId, id);
            if (farmWorker == null || farmWorker.Role != WorkerRole.Owner)
                return Forbid();

            _context.Farms.Remove(farm);
            await _context.SaveChangesAsync();

            var user = await _context.Users.FindAsync(currentUserId);
            if (user.DefaultFarmId == farm.Id)
            {
                var farmWorkersList = await _context.FarmWorkers.Where(l => l.UserId == user.Id).ToListAsync();
                if (farmWorkersList.Any())
                {
                    var farm2 = await _context.Farms.FindAsync(farmWorkersList.First().FarmId);
                    user.DefaultFarmId = farm2.Id;
                    _context.Entry(user).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                }
                else
                {
                    user.DefaultFarmId = null;
                    _context.Entry(user).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                }
            }
            return _mapper.Map<FarmReadDTO>(farm);
        }
    }
}
