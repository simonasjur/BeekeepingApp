using AutoMapper;
using BeekeepingApi.DTOs.SuperDTOs;
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
    public class SupersController : ControllerBase
    {
        private readonly BeekeepingContext _context;
        private readonly IMapper _mapper;

        public SupersController(BeekeepingContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        //GET: api/supers/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<SuperReadDTO>> GetSuper(long id)
        {
            var super = await _context.Supers.FindAsync(id);
            if (super == null)
            {
                return NotFound();
            }

            var beehive = await _context.Beehives.FindAsync(super.BeehiveId);
            if (beehive == null)
            {
                return NotFound();
            }

            var currentUserId = long.Parse(User.Identity.Name);
            var farmWorker = await _context.FarmWorkers.FindAsync(currentUserId, beehive.FarmId);
            if (farmWorker == null)
            {
                return Forbid();
            }

            return _mapper.Map<SuperReadDTO>(super);
        }

        //GET: api/beehives/{beehiveId}/supers
        [HttpGet("/api/beehives/{beehiveId}/supers")]
        public async Task<ActionResult<IEnumerable<SuperReadDTO>>> GetBeehiveSupers(long beehiveId)
        {
            var beehive = await _context.Beehives.FindAsync(beehiveId);
            if (beehive == null)
            {
                return NotFound();
            }

            var currentUserId = long.Parse(User.Identity.Name);
            var farmWorker = await _context.FarmWorkers.FindAsync(currentUserId, beehive.FarmId);
            if (farmWorker == null)
            {
                return Forbid();
            }

            var supers = await _context.Supers.Where(s => s.BeehiveId == beehiveId).ToListAsync();

            return _mapper.Map<IEnumerable<SuperReadDTO>>(supers).ToList();
        }

        //POST: api/supers
        [HttpPost]
        public async Task<ActionResult<SuperReadDTO>> CreateSuper(SuperCreateDTO superCreateDTO)
        {
            var beehive = await _context.Beehives.FindAsync(superCreateDTO.BeehiveId);
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

            var super = _mapper.Map<Super>(superCreateDTO);
            _context.Supers.Add(super);
            await _context.SaveChangesAsync();

            var superReadDTO = _mapper.Map<SuperReadDTO>(super);
            return CreatedAtAction(nameof(GetSuper), new { id = super.Id }, superReadDTO);
        }

        //PUT: api/supers/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSuper(long id, SuperEditDTO superEditDTO)
        {
            if (id != superEditDTO.Id)
            {
                return BadRequest();
            }

            var super = await _context.Supers.FindAsync(id);
            if (super == null)
            {
                return BadRequest();
            }

            var beehive = await _context.Beehives.FindAsync(super.BeehiveId);
            var currentUserId = long.Parse(User.Identity.Name);
            var farmWorker = await _context.FarmWorkers.FindAsync(currentUserId, beehive.FarmId);
            if (farmWorker == null)
            {
                return Forbid();
            }

            _mapper.Map(superEditDTO, super);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        //DELETE: api/supers/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult<SuperReadDTO>> DeleteSuper(long id)
        {
            var super = await _context.Supers.FindAsync(id);
            if (super == null)
            {
                return NotFound();
            }

            var beehive = await _context.Beehives.FindAsync(super.BeehiveId);
            var currentUserId = long.Parse(User.Identity.Name);
            var farmWorker = await _context.FarmWorkers.FindAsync(currentUserId, beehive.FarmId);
            if (farmWorker == null)
            {
                return Forbid();
            }

            _context.Supers.Remove(super);
            await _context.SaveChangesAsync();

            return _mapper.Map<SuperReadDTO>(super);
        }
    }
}
