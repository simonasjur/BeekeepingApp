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
    [Route("api/Farms/{farmId}/[controller]")]
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

        // GET: api/Farms/1/FarmWorkers
        [HttpGet]
        [EnableQuery()]
        public async Task<ActionResult<IEnumerable<FarmWorkerReadDTO>>> GetFarmWorkers(long id)
        {
            var farm = await _context.Farms.FindAsync(id);
            if (farm == null)
                return NotFound();

            var currentUserId = long.Parse(User.Identity.Name);
            var farmWorkersList = await _context.FarmWorkers.Where(l => l.FarmId == id).ToListAsync();

            if (farmWorkersList.Any() && farmWorkersList.First().UserId != currentUserId)
                return Forbid();

            return _mapper.Map<IEnumerable<FarmWorkerReadDTO>>(farmWorkersList).ToList();
        }

        // DELETE: api/Users/1/FarmWorkers/1
        [HttpDelete("{workerId}")]
        public async Task<ActionResult<FarmWorkerReadDTO>> DeleteFarmWorker(long farmId, long workerId)
        {
            var currentUserId = long.Parse(User.Identity.Name);
            if (workerId != currentUserId)
                return Forbid();

            var farmWorker = await _context.FarmWorkers.FindAsync(workerId, farmId);
            if (farmWorker == null)
                return NotFound();

            _context.FarmWorkers.Remove(farmWorker);
            await _context.SaveChangesAsync();

            return _mapper.Map<FarmWorkerReadDTO>(farmWorker);
        }
    }
}