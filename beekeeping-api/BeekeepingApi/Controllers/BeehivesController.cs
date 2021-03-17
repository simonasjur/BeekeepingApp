using AutoMapper;
using BeekeepingApi.DTOs.BeehiveDTOs;
using BeekeepingApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeekeepingApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BeehivesController : ControllerBase
    {
        private readonly BeekeepingContext _context;
        private readonly IMapper _mapper;

        public BeehivesController(BeekeepingContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        //GET: api/beehives/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<BeehiveReadDTO>> GetBeehive(long id)
        {           
            var beehive = await _context.Beehives.FindAsync(id);
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

            return _mapper.Map<BeehiveReadDTO>(beehive);
        }

        //POST: api/beehives
        [HttpPost]
        public async Task<ActionResult<BeehiveReadDTO>> CreateBeehive(BeehiveCreateDTO beehiveCreateDTO)
        {
            var currentUserId = long.Parse(User.Identity.Name);
            var farmWorker = await _context.FarmWorkers.FindAsync(currentUserId, beehiveCreateDTO.FarmId);
            if (farmWorker == null)
            {
                return Forbid();
            }

            var beehive = _mapper.Map<Beehive>(beehiveCreateDTO);
            _context.Beehives.Add(beehive);
            await _context.SaveChangesAsync();

            var beehiveReadDTO = _mapper.Map<BeehiveReadDTO>(beehive);
            return CreatedAtAction(nameof(GetBeehive), new { id = beehive.Id }, beehiveReadDTO);
        }

        // PUT: api/beehives/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBeehive(long id, BeehiveEditDTO beehiveEditDTO)
        {
            if (id != beehiveEditDTO.Id)
            {
                return BadRequest();
            }

            var currentUserId = long.Parse(User.Identity.Name);
            var farmWorker = await _context.FarmWorkers.FindAsync(currentUserId, beehiveEditDTO.FarmId);
            if (farmWorker == null)
            {
                return Forbid();
            }

            var beehive = await _context.Beehives.FindAsync(id);
            if (beehive == null)
            {
                return NotFound();
            }

            _mapper.Map(beehiveEditDTO, beehive);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        //DELETE: api/beehives/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult<BeehiveReadDTO>> DeleteBeehive(long id)
        {
            var beehive = await _context.Beehives.FindAsync(id);
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

            _context.Beehives.Remove(beehive);
            await _context.SaveChangesAsync();

            return _mapper.Map<BeehiveReadDTO>(beehive);
        }
    }
}