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
using BeekeepingApi.DTOs.FarmWorkerDTOs;

namespace BeekeepingApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
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
        public async Task<ActionResult<IEnumerable<FarmWorkerReadDTO>>> GetFarmWorker()
        {
            var farmWorkersList = await _context.FarmWorker.ToListAsync();

            return _mapper.Map<IEnumerable<FarmWorkerReadDTO>>(farmWorkersList).ToList();
        }

        // GET: api/FarmWorkers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<FarmWorkerReadDTO>> GetFarmWorker(long id)
        {
            var farmWorker = await _context.FarmWorker.FindAsync(id);

            var currentUserId = long.Parse(User.Identity.Name);

            if (farmWorker.UserId != currentUserId)
            {
                return Forbid();
            }    

            if (farmWorker == null)
            {
                return NotFound();
            }

            return _mapper.Map<FarmWorkerReadDTO>(farmWorker);
        }

        // PUT: api/FarmWorkers/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFarmWorker(long id, FarmWorker farmWorker)
        {
            if (id != farmWorker.Id)
            {
                return BadRequest();
            }

            _context.Entry(farmWorker).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FarmWorkerExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/FarmWorkers
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<FarmWorker>> PostFarmWorker(FarmWorker farmWorker)
        {
            _context.FarmWorker.Add(farmWorker);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFarmWorker", new { id = farmWorker.Id }, farmWorker);
        }

        // DELETE: api/FarmWorkers/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<FarmWorker>> DeleteFarmWorker(long id)
        {
            var farmWorker = await _context.FarmWorker.FindAsync(id);
            if (farmWorker == null)
            {
                return NotFound();
            }

            _context.FarmWorker.Remove(farmWorker);
            await _context.SaveChangesAsync();

            return farmWorker;
        }

        private bool FarmWorkerExists(long id)
        {
            return _context.FarmWorker.Any(e => e.Id == id);
        }
    }
}
