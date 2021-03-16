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

        // GET: api/Farms/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Farm>> GetFarm(long id)
        {
            var farm = await _context.Farms.FindAsync(id);

            if (farm == null)
            {
                return NotFound();
            }

            return farm;
        }

        // PUT: api/Farms/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFarm(long id, Farm farm)
        {
            if (id != farm.Id)
            {
                return BadRequest();
            }

            _context.Entry(farm).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FarmExists(id))
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

        // POST: api/Farms
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Farm>> PostFarm(Farm farm)
        {
            _context.Farms.Add(farm);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFarm", new { id = farm.Id }, farm);
        }

        // DELETE: api/Farms/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Farm>> DeleteFarm(long id)
        {
            var farm = await _context.Farms.FindAsync(id);
            if (farm == null)
            {
                return NotFound();
            }

            _context.Farms.Remove(farm);
            await _context.SaveChangesAsync();

            return farm;
        }

        private bool FarmExists(long id)
        {
            return _context.Farms.Any(e => e.Id == id);
        }
    }
}
