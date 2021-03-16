using AutoMapper;
using BeekeepingApi.DTOs.FarmWorkerDTOs;
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
    [Route("api/Users/{userId}/[controller]")]
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

        // GET: api/Users/1/FarmWorkers
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FarmWorkerReadDTO>>> GetFarmWorkers(long userId)
        {
            var currentUserId = long.Parse(User.Identity.Name);
            if (userId != currentUserId)
                return Forbid();

            var user = await _context.Users.FindAsync(userId);

            var farmWorkersList = await _context.FarmWorkers.Where(l => l.UserId == userId).ToListAsync();

            return _mapper.Map<IEnumerable<FarmWorkerReadDTO>>(farmWorkersList).ToList();
        }

        // DELETE: api/Users/5/FarmWorkers/1
        [Authorize]
        [HttpDelete("{workerId}")]
        public async Task<ActionResult<FarmWorkerReadDTO>> DeleteFarmWorker(long userId, long workerId)
        {
            var currentUserId = long.Parse(User.Identity.Name);
            if (userId != currentUserId)
                return Forbid();

            var farmWorker = await _context.FarmWorkers.FindAsync(userId, workerId);
            if (farmWorker == null)
                return NotFound();

            if (farmWorker.UserId != currentUserId)
                return Forbid();

            _context.FarmWorkers.Remove(farmWorker);
            await _context.SaveChangesAsync();

            return _mapper.Map<FarmWorkerReadDTO>(farmWorker);
        }
    }
}