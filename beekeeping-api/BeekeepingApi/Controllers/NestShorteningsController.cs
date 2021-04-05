using AutoMapper;
using BeekeepingApi.DTOs.NestShorteningDTOs;
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
    public class NestShorteningsController : ControllerBase
    {
        private readonly BeekeepingContext _context;
        private readonly IMapper _mapper;

        public NestShorteningsController(BeekeepingContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        //GET: api/NestShortenings/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<NestShorteningReadDTO>> GetNestShortening(long id)
        {
            var nestShortening = await _context.NestShortenings.FindAsync(id);
            if (nestShortening == null)
            {
                return NotFound();
            }

            var beehive = await _context.Beehives.FindAsync(nestShortening.BeehiveId);
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

            return _mapper.Map<NestShorteningReadDTO>(nestShortening);
        }

        //GET: api/Beehives/{beehiveId}/NestShortenings
        [HttpGet("/api/Beehives/{beehiveId}/NestShortenings")]
        public async Task<ActionResult<IEnumerable<NestShorteningReadDTO>>> GetBeehiveNestShortenings(long beehiveId)
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

            var nestShortenings = await _context.NestShortenings.Where(ns => ns.BeehiveId == beehiveId).ToListAsync();

            return _mapper.Map<IEnumerable<NestShorteningReadDTO>>(nestShortenings).ToList();
        }

        //POST: api/NestShortenings
        [HttpPost]
        public async Task<ActionResult<NestShorteningReadDTO>> CreateNestShortening(NestShorteningCreateDTO nestShorteningCreateDTO)
        {
            var beehive = await _context.Beehives.FindAsync(nestShorteningCreateDTO.BeehiveId);
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

            var nestShortening = _mapper.Map<NestShortening>(nestShorteningCreateDTO);

            //If new nest shortening done current year and was latest, beehive combs value is changed and old value is stored to new nest shortening
            var currentYear = DateTime.Now.Year;
            if (nestShorteningCreateDTO.Date.Year == currentYear)
            {
                await _context.Entry(beehive).Collection(b => b.NestShortenings).LoadAsync();
                var lastNestShortening = beehive.NestShortenings.OrderByDescending(ns => ns.Date).FirstOrDefault();
                if (lastNestShortening == null ||
                    DateTime.Compare(nestShorteningCreateDTO.Date, lastNestShortening.Date) > 0)
                {
                    nestShortening.CombsBefore = beehive.NestCombs;
                    beehive.NestCombs = nestShorteningCreateDTO.StayedCombs;
                    _context.Entry(beehive).State = EntityState.Modified;
                }
            }

            _context.NestShortenings.Add(nestShortening);
            await _context.SaveChangesAsync();

            var nestShorteningReadDTO = _mapper.Map<NestShorteningReadDTO>(nestShortening);
            return CreatedAtAction(nameof(GetNestShortening), new { id = nestShortening.Id }, nestShorteningReadDTO);
        }

        //PUT: api/NestShortenings/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateNestShortening(long id, NestShorteningEditDTO nestShorteningEditDTO)
        {
            if (id != nestShorteningEditDTO.Id)
            {
                return BadRequest();
            }

            var nestShortening = await _context.NestShortenings.FindAsync(id);
            if (nestShortening == null)
            {
                return BadRequest();
            }

            var beehive = await _context.Beehives.FindAsync(nestShortening.BeehiveId);
            var currentUserId = long.Parse(User.Identity.Name);
            var farmWorker = await _context.FarmWorkers.FindAsync(currentUserId, beehive.FarmId);
            if (farmWorker == null)
            {
                return Forbid();
            }

            var lastNestShortening = await _context.NestShortenings.Where(ns => ns.BeehiveId == beehive.Id)
                                                                   .OrderByDescending(ns => ns.Date)
                                                                   .FirstOrDefaultAsync();
            //If edited nest shortening is not this year latest and have information about combs before nest shortening,
            //this object cannot be edited
            if (DateTime.Now.Year == nestShortening.Date.Year &&
                nestShortening.CombsBefore != null &&
                nestShortening.Id != lastNestShortening.Id)
            {
                return BadRequest("If you want to change this nest shortening data, " +
                                  "you must delete this beehive latest nest shortenings first");
            }
            //If edited nest shortening is latest, beehive combs value is changed
            if (lastNestShortening.Id == id &&
                lastNestShortening.StayedCombs != nestShorteningEditDTO.StayedCombs)
            {
                beehive.NestCombs = nestShorteningEditDTO.StayedCombs;
                _context.Entry(beehive).State = EntityState.Modified;
            }

            _mapper.Map(nestShorteningEditDTO, nestShortening);

            await _context.SaveChangesAsync();

            return NoContent();
        }

        //DELETE: api/NestShortenings/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult<NestShorteningReadDTO>> DeleteNestShortening(long id)
        {
            var nestShortening = await _context.NestShortenings.FindAsync(id);
            if (nestShortening == null)
            {
                return NotFound();
            }

            var beehive = await _context.Beehives.FindAsync(nestShortening.BeehiveId);
            var currentUserId = long.Parse(User.Identity.Name);
            var farmWorker = await _context.FarmWorkers.FindAsync(currentUserId, beehive.FarmId);
            if (farmWorker == null)
            {
                return Forbid();
            }

            var nestShortenings = await _context.NestShortenings.Where(ns => ns.BeehiveId == beehive.Id)
                                                                .OrderByDescending(ns => ns.Date)
                                                                .ToListAsync();
            var lastNestShortening = nestShortenings.ElementAtOrDefault(0);

            //If deleted nest shortening done current year but not latest and 
            //have information about combs before nest shortening, this object cannot be deleted
            if (DateTime.Now.Year == nestShortening.Date.Year && 
                lastNestShortening.Id != id &&
                nestShortening.CombsBefore != null)
            {
                return BadRequest("If you want to delete this nest shortening, " +
                                  "you must delete this beehive latest nest shortenings first");
            }

            _context.NestShortenings.Remove(nestShortening);

            //If deleted nest shortening done current year and was latest, beehive combs value is changed to old value
            if (DateTime.Now.Year == nestShortening.Date.Year && lastNestShortening.Id == id)
            {
                var nestCombs = nestShortening.CombsBefore;
                var secondLastNestShortening = nestShortenings.ElementAtOrDefault(1);
                if (secondLastNestShortening != null && 
                    DateTime.Now.Year == secondLastNestShortening.Date.Year && 
                    secondLastNestShortening.CombsBefore == null)
                {
                    nestCombs = secondLastNestShortening.StayedCombs;
                    secondLastNestShortening.CombsBefore = nestShortening.CombsBefore;
                }
                beehive.NestCombs = nestCombs;
                _context.Entry(beehive).State = EntityState.Modified;
            }

            await _context.SaveChangesAsync();

            return _mapper.Map<NestShorteningReadDTO>(nestShortening);
        }
    }
}
