using AutoMapper;
using BeekeepingApi.DTOs.QueensRaisingDTOs;
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
    public class QueensRaisingsController : ControllerBase
    {
        private readonly BeekeepingContext _context;
        private readonly IMapper _mapper;

        public QueensRaisingsController(BeekeepingContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Farms/1/QueensRaisings
        [HttpGet("/api/Farms/{farmId}/QueensRaisings")]
        [EnableQuery()]
        public async Task<ActionResult<IEnumerable<QueensRaisingReadDTO>>> GetFarmQueensRaisings(long farmId)
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

            var queensRaisingsList = await _context.QueensRaisings.Where(qr => qr.Mother.FarmId == farmId).ToListAsync();
            foreach (var queensRaising in queensRaisingsList)
            {
                await _context.Entry(queensRaising).Reference(qr => qr.Mother).LoadAsync();
                await _context.Entry(queensRaising).Reference(qr => qr.Beefamily).LoadAsync();
            }

            return _mapper.Map<IEnumerable<QueensRaisingReadDTO>>(queensRaisingsList).ToList();
        }

        // GET: api/QueensRaisings/1
        [HttpGet("{id}")]
        public async Task<ActionResult<QueensRaisingReadDTO>> GetQueensRaising(long id)
        {
            var queensRaising = await _context.QueensRaisings.FindAsync(id);
            if (queensRaising == null)
            {
                return NotFound();
            }

            await _context.Entry(queensRaising).Reference(qr => qr.Mother).LoadAsync();
            var currentUserId = long.Parse(User.Identity.Name);
            var farmWorker = await _context.FarmWorkers.FindAsync(currentUserId, queensRaising.Mother.FarmId);
            if (farmWorker == null)
            {
                return Forbid();
            }

            await _context.Entry(queensRaising).Reference(qr => qr.Beefamily).LoadAsync();
            return _mapper.Map<QueensRaisingReadDTO>(queensRaising);
        }

        // POST: api/QueensRaisings
        [HttpPost]
        public async Task<ActionResult<QueensRaisingReadDTO>> CreateQueensRaising(QueensRaisingCreateDTO queensRaisingCreateDTO)
        {
            var queen = await _context.Queens.FindAsync(queensRaisingCreateDTO.MotherId);
            if (queen == null)
            {
                return BadRequest();
            }

            var beefamily = await _context.BeeFamilies.FindAsync(queensRaisingCreateDTO.BeefamilyId);
            if (beefamily == null || queen.FarmId != beefamily.FarmId)
            {
                return BadRequest();
            }

            var currentUserId = long.Parse(User.Identity.Name);
            var farmWorker = await _context.FarmWorkers.FindAsync(currentUserId, queen.FarmId);
            if (farmWorker == null)
            {
                return Forbid();
            }
            
            //Only living queen can be mother 
            if (queen.State != QueenState.GyvenaAvilyje)
            {
                return BadRequest();
            }

            if (!IsDevelopmentPlaceValid(queensRaisingCreateDTO.DevelopmentPlace ?? DevelopmentPlace.Beehive))
            {
                return BadRequest("Invalid queens development place");
            }

            var queensRaising = _mapper.Map<QueensRaising>(queensRaisingCreateDTO);
            _context.QueensRaisings.Add(queensRaising);
            await _context.SaveChangesAsync();

            var queensRaisingReadDTO = _mapper.Map<QueensRaisingReadDTO>(queensRaising);

            return CreatedAtAction("GetQueensRaising", "QueensRaisings", new { id = queensRaising.Id }, queensRaisingReadDTO);
        }

        // PUT: api/QueensRaisings/1
        [HttpPut("{id}")]
        public async Task<IActionResult> EditQueensRaising(long id, QueensRaisingEditDTO queensRaisingEditDTO)
        {
            if (id != queensRaisingEditDTO.Id)
            {
                return BadRequest();
            }

            var queensRaising = await _context.QueensRaisings.FindAsync(id);
            if (queensRaising == null)
            {
                return NotFound();
            }

            await _context.Entry(queensRaising).Reference(qr => qr.Mother).LoadAsync();
            var farm = await _context.Farms.FindAsync(queensRaising.Mother.FarmId);
            if (farm == null)
            {
                return NotFound();
            }

            var currentUserId = long.Parse(User.Identity.Name);
            var farmWorker = await _context.FarmWorkers.FindAsync(currentUserId, farm.Id);
            if (farmWorker == null)
            {
                return Forbid();
            }
            
            if (!IsDevelopmentPlaceValid(queensRaisingEditDTO.DevelopmentPlace ?? DevelopmentPlace.Beehive))
            {
                return BadRequest("Invalid queens development place");
            }

            if (queensRaisingEditDTO.LarvaCount < queensRaisingEditDTO.CappedCellCount ||
                queensRaisingEditDTO.CappedCellCount < queensRaisingEditDTO.QueensCount)
            {
                return BadRequest("Invalid data");
            }

            _mapper.Map(queensRaisingEditDTO, queensRaising);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/QueensRaisings/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<QueensRaisingReadDTO>> DeleteQueensRaising(long id)
        {
            var queensRaising = await _context.QueensRaisings.FindAsync(id);
            if (queensRaising == null)
            {
                return NotFound();
            }

            await _context.Entry(queensRaising).Reference(qr => qr.Mother).LoadAsync();
            var currentUserId = long.Parse(User.Identity.Name);
            var farmWorker = await _context.FarmWorkers.FindAsync(currentUserId, queensRaising.Mother.FarmId);
            if (farmWorker == null)
            {
                return Forbid();
            }

            _context.QueensRaisings.Remove(queensRaising);
            await _context.SaveChangesAsync();

            return _mapper.Map<QueensRaisingReadDTO>(queensRaising);
        }

        private bool IsDevelopmentPlaceValid(DevelopmentPlace place)
        {
            return place == DevelopmentPlace.Beehive || place == DevelopmentPlace.Incubator;
        }
    }
}
