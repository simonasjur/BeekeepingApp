using AutoMapper;
using BeekeepingApi.DTOs.BeehiveDTOs;
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

        //GET: api/farms/{farmId}/beehives
        [HttpGet("/api/farms/{farmId}/beehives")]
        public async Task<ActionResult<IEnumerable<BeehiveReadDTO>>> GetFarmBeehives(long farmId)
        {
            var farm = await _context.Farms.FindAsync(farmId);
            if (farm == null)
            {
                return NotFound();
            }

            var currentUserId = long.Parse(User.Identity.Name);
            var farmWorker = await _context.FarmWorkers.FindAsync(currentUserId, farmId);
            if (farmWorker == null)
            {
                return Forbid();
            }

            var beehives = await _context.Beehives.Where(b => b.FarmId == farmId).ToListAsync();

            return _mapper.Map<IEnumerable<BeehiveReadDTO>>(beehives).ToList();
        }

        //POST: api/beehives
        [HttpPost]
        public async Task<ActionResult<BeehiveReadDTO>> CreateBeehive(BeehiveCreateDTO beehiveCreateDTO)
        {
            if (beehiveCreateDTO.Type == BeehiveTypes.Daugiaaukštis && beehiveCreateDTO.NestCombs != null)
            {
                return BadRequest();
            }

            var farm = await _context.Farms.FindAsync(beehiveCreateDTO.FarmId);
            if (farm == null)
            {
                return BadRequest();
            }

            var currentUserId = long.Parse(User.Identity.Name);
            var farmWorker = await _context.FarmWorkers.FindAsync(currentUserId, farm.Id);
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

            await _context.Entry(beehive).Collection(b => b.ApiaryBeehives).LoadAsync();

            _context.Beehives.Remove(beehive);
            await _context.SaveChangesAsync();

            return _mapper.Map<BeehiveReadDTO>(beehive);
        }
    }
}