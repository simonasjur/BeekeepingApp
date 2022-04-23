using AutoMapper;
using BeekeepingApi.DTOs.FarmWorkerDTOs;
using BeekeepingApi.Models;
using Microsoft.AspNet.OData;
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
    public class FarmWorkersController : ControllerBase
    {
        private readonly BeekeepingContext _context;
        private readonly IMapper _mapper;

        public FarmWorkersController(BeekeepingContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/FarmWorkers
        [HttpGet]
        [EnableQuery()]
        public async Task<ActionResult<IEnumerable<FarmWorkerReadDTO>>> GetFarmWorkers()
        {
            var currentUserId = long.Parse(User.Identity.Name);
            var farmWorkersList = await _context.FarmWorkers.Where(l => l.UserId == currentUserId).ToListAsync();

            return _mapper.Map<IEnumerable<FarmWorkerReadDTO>>(farmWorkersList).ToList();
        }

        [HttpGet("/api/Farms/{farmId}/Farmworkers")]
        [EnableQuery()]
        public async Task<ActionResult<IEnumerable<FarmWorkerReadDTO>>> GetFarmFarmWorkers(long farmId)
        {
            var farm = await _context.Farms.FindAsync(farmId);
            if (farm == null)
                return NotFound();

            var currentUserId = long.Parse(User.Identity.Name);
            var farmWorker = await _context.FarmWorkers.FindAsync(currentUserId, farmId);
            if (farmWorker == null || farmWorker.Role != WorkerRole.Owner)
                return Forbid();

            var farmWorkersList = await _context.FarmWorkers.Where(l => l.FarmId == farmId && l.UserId != currentUserId).ToListAsync();
            foreach (var worker in farmWorkersList)
            {
                var user = await _context.Users.FindAsync(worker.UserId);
                worker.FirstName = user.FirstName;
                worker.LastName = user.LastName;
            }

            return _mapper.Map<IEnumerable<FarmWorkerReadDTO>>(farmWorkersList).ToList();
        }


        [HttpGet("/api/Farms/{farmId}/Farmworker")]
        public async Task<ActionResult<FarmWorkerReadDTO>> GetFarmAndUserWorker(long farmId)
        {
            var farm = await _context.Farms.FindAsync(farmId);
            if (farm == null)
                return NotFound();

            var currentUserId = long.Parse(User.Identity.Name);

            var farmWorkers = await _context.FarmWorkers.Where(l => l.FarmId == farmId && l.UserId == currentUserId).ToListAsync();
            

            if (farmWorkers.Any() && farmWorkers.First().UserId != currentUserId)
                return Forbid();

            return _mapper.Map<FarmWorkerReadDTO>(farmWorkers.First());
        }

        // DELETE: api/Farms/1/FarmWorkers/1
        [HttpDelete("api/Farms/{farmId}/Farmworkers/{userId}")]
        public async Task<ActionResult<FarmWorkerReadDTO>> DeleteFarmWorker(long farmId, long userId)
        {
            return Forbid();
            var farm = await _context.Farms.FindAsync(farmId);
            if (farm == null)
                return NotFound();

            var currentUserId = long.Parse(User.Identity.Name);
            var farmWorker = await _context.FarmWorkers.FindAsync(currentUserId, farmId);
            if (farmWorker == null || farmWorker.Role != WorkerRole.Owner)
                return Forbid();

            var farmWorkerToDelete = await _context.FarmWorkers.FindAsync(userId, farmId);
            if (farmWorkerToDelete == null)
                return NotFound();

            _context.FarmWorkers.Remove(farmWorkerToDelete);
            await _context.SaveChangesAsync();

            return _mapper.Map<FarmWorkerReadDTO>(farmWorkerToDelete);
        }
    }
}