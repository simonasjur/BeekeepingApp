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
    public class NestReductionsController : ControllerBase
    {
        private readonly BeekeepingContext _context;
        private readonly IMapper _mapper;

        public NestReductionsController(BeekeepingContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        
        //GET: api/NestReductions/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<NestReductionReadDTO>> GetNestReduction(long id)
        {
            var nestReduction = await _context.NestReductions.FindAsync(id);
            if (nestReduction == null)
            {
                return NotFound();
            }

            var beefamily = await _context.BeeFamilies.FindAsync(nestReduction.BeefamilyId);
            if (beefamily == null)
            {
                return NotFound();
            }

            var currentUserId = long.Parse(User.Identity.Name);
            var farmWorker = await _context.FarmWorkers.FindAsync(currentUserId, beefamily.FarmId);
            if (farmWorker == null)
            {
                return Forbid();
            }

            return _mapper.Map<NestReductionReadDTO>(nestReduction);
        }

        //GET: api/BeeFamilies/{beefamilyId}/NestReductions
        [HttpGet("/api/BeeFamilies/{beefamilyId}/NestReductions")]
        public async Task<ActionResult<IEnumerable<NestReductionReadDTO>>> GetBeefamilyNestReductions(long beefamilyId)
        {
            var beefamily = await _context.BeeFamilies.FindAsync(beefamilyId);
            if (beefamily == null)
            {
                return NotFound();
            }

            var currentUserId = long.Parse(User.Identity.Name);
            var farmWorker = await _context.FarmWorkers.FindAsync(currentUserId, beefamily.FarmId);
            if (farmWorker == null)
            {
                return Forbid();
            }

            var nestReductions = await _context.NestReductions.Where(nr => nr.BeefamilyId == beefamilyId)
                                                              .OrderByDescending(nr => nr.Date).ToListAsync();

            return _mapper.Map<IEnumerable<NestReductionReadDTO>>(nestReductions).ToList();
        }

        //POST: api/NestReductions
        [HttpPost]
        public async Task<ActionResult<NestReductionReadDTO>> CreateNestReduction(NestReductionCreateDTO nestReductionCreateDTO)
        {
            var beefamily = await _context.BeeFamilies.FindAsync(nestReductionCreateDTO.BeefamilyId);
            if (beefamily == null || beefamily.State == BeeFamilyStates.Išmirusi || beefamily.State == BeeFamilyStates.SujungtaSuKitaŠeima)
            {
                return BadRequest();
            }

            var currentUserId = long.Parse(User.Identity.Name);
            var farmWorker = await _context.FarmWorkers.FindAsync(currentUserId, beefamily.FarmId);
            if (farmWorker == null || farmWorker.Permissions[15] != '1')
            {
                return Forbid();
            }

            var beehiveBeefamily = await _context.BeehiveBeeFamilies.FirstOrDefaultAsync(bb => bb.BeeFamilyId == beefamily.Id &&
                                                                                                       bb.DepartDate == null);
            if (beehiveBeefamily == null)
            {
                return BadRequest();
            }
            var beehive = await _context.Beehives.FindAsync(beehiveBeefamily.BeehiveId);
            if (beehive == null)
            {
                return BadRequest();
            }
            var nestReduction = _mapper.Map<NestReduction>(nestReductionCreateDTO);

            //If new nest reduction done current year and was latest, beehive combs value is changed and old value is stored to new nest reduction
            var currentYear = DateTime.Now.Year;
            DateTime reductionDate = nestReductionCreateDTO.Date ?? DateTime.Today;
            if (reductionDate.Year == currentYear)
            {
                
                await _context.Entry(beefamily).Collection(b => b.NestReductions).LoadAsync();
                var lastNestReduction = beefamily.NestReductions.OrderByDescending(ns => ns.Date).FirstOrDefault();
                if (lastNestReduction == null ||
                    DateTime.Compare(reductionDate, lastNestReduction.Date) > 0)
                {
                    if (beehive.Type == BeehiveTypes.Dadano)
                    {
                        nestReduction.CombsBefore = beehive.NestCombs;
                        beehive.NestCombs = nestReductionCreateDTO.StayedCombs;
                        _context.Entry(beehive).State = EntityState.Modified;
                    } 
                }
                else
                {
                    return BadRequest("Invalid date");
                }
            }

            _context.NestReductions.Add(nestReduction);
            await _context.SaveChangesAsync();

            var nestReductionReadDTO = _mapper.Map<NestReductionReadDTO>(nestReduction);
            return CreatedAtAction(nameof(GetNestReduction), new { id = nestReduction.Id }, nestReductionReadDTO);
        }

        //PUT: api/NestReductions/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateNestReduction(long id, NestReductionEditDTO nestReductionEditDTO)
        {
            if (id != nestReductionEditDTO.Id)
            {
                return BadRequest();
            }

            var nestReduction = await _context.NestReductions.FindAsync(id);
            if (nestReduction == null)
            {
                return BadRequest();
            }

            var beefamily = await _context.BeeFamilies.FindAsync(nestReduction.BeefamilyId);
            var currentUserId = long.Parse(User.Identity.Name);
            var farmWorker = await _context.FarmWorkers.FindAsync(currentUserId, beefamily.FarmId);
            if (farmWorker == null || farmWorker.Permissions[16] != '1')
            {
                return Forbid();
            }

            var newNestReductionDate = nestReductionEditDTO.Date ?? DateTime.Today;
            if (newNestReductionDate.Year != nestReduction.Date.Year)
            {
                return BadRequest("Can't change date year");
            }

            //Beehive nest combs can be changed if beefamily is still alive and reduction done this year
            if (DateTime.Now.Year == nestReduction.Date.Year && 
                beefamily.State != BeeFamilyStates.Išmirusi && beefamily.State != BeeFamilyStates.SujungtaSuKitaŠeima)
            {
                var beehiveBeefamily = await _context.BeehiveBeeFamilies.FirstOrDefaultAsync(bb => bb.BeeFamilyId == beefamily.Id &&
                                                                                                       bb.DepartDate == null);
                if (beehiveBeefamily != null)
                {
                    var beehive = await _context.Beehives.FindAsync(beehiveBeefamily.BeehiveId);
                    if (beehive != null)
                    {
                        if (beehive.Type == BeehiveTypes.Dadano)
                        {
                            var lastNestReduction = await _context.NestReductions.Where(nr => nr.BeefamilyId == beefamily.Id)
                                                                               .OrderByDescending(nr => nr.Date)
                                                                               .FirstOrDefaultAsync();
                            //If edited nest reduction is not this year latest and have information about combs before nest reduction,
                            //this object cannot be edited
                            if (nestReduction.CombsBefore != null &&
                                nestReduction.Id != lastNestReduction.Id)
                            {
                                return BadRequest("If you want to change this nest reduction data, " +
                                                  "you must delete this beefamily latest nest reductions first");
                            }
                            //If edited nest reduction is latest, beehive combs value is changed
                            if (lastNestReduction.Id == id &&
                                lastNestReduction.StayedCombs != nestReductionEditDTO.StayedCombs)
                            {
                                beehive.NestCombs = nestReductionEditDTO.StayedCombs;
                                _context.Entry(beehive).State = EntityState.Modified;
                                await _context.SaveChangesAsync();
                            }
                        }
                    }
                }
            }

            _mapper.Map(nestReductionEditDTO, nestReduction);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        //DELETE: api/NestReductions/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult<NestReductionReadDTO>> DeleteNestReduction(long id)
        {
            var nestReduction = await _context.NestReductions.FindAsync(id);
            if (nestReduction == null)
            {
                return NotFound();
            }

            var beefamily = await _context.BeeFamilies.FindAsync(nestReduction.BeefamilyId);
            var currentUserId = long.Parse(User.Identity.Name);
            var farmWorker = await _context.FarmWorkers.FindAsync(currentUserId, beefamily.FarmId);
            if (farmWorker == null || farmWorker.Permissions[17] != '1')
            {
                return Forbid();
            }

            var nestReductions = await _context.NestReductions.Where(nr => nr.BeefamilyId == beefamily.Id)
                                                                .OrderByDescending(nr => nr.Date)
                                                                .ToListAsync();
            var lastNestReduction = nestReductions.ElementAtOrDefault(0);

            //If deleted nest shortening done current year but not latest and 
            //have information about combs before nest shortening, this object cannot be deleted
            if (DateTime.Now.Year == nestReduction.Date.Year && 
                lastNestReduction.Id != id &&
                nestReduction.CombsBefore != null)
            {
                return BadRequest("If you want to delete this nest shortening, " +
                                  "you must delete this beehive latest nest shortenings first");
            }

            _context.NestReductions.Remove(nestReduction);

            //If deleted nest reduction done current year and was latest and beefamily is still alive then beehive combs value is changed to old value
            if (DateTime.Now.Year == nestReduction.Date.Year && lastNestReduction.Id == id &&
                beefamily.State != BeeFamilyStates.Išmirusi && beefamily.State != BeeFamilyStates.SujungtaSuKitaŠeima)
            {
                var beehiveBeefamily = await _context.BeehiveBeeFamilies.FirstOrDefaultAsync(bb => bb.BeeFamilyId == beefamily.Id &&
                                                                                                       bb.DepartDate == null);
                if (beehiveBeefamily != null)
                {
                    var beehive = await _context.Beehives.FindAsync(beehiveBeefamily.BeehiveId);
                    if (beehive != null)
                    {
                        if (beehive.Type == BeehiveTypes.Dadano)
                        {
                            var nestCombs = nestReduction.CombsBefore;
                            var secondLastNestShortening = nestReductions.ElementAtOrDefault(1);
                            if (secondLastNestShortening != null &&
                                DateTime.Now.Year == secondLastNestShortening.Date.Year &&
                                secondLastNestShortening.CombsBefore == null)
                            {
                                nestCombs = secondLastNestShortening.StayedCombs;
                                secondLastNestShortening.CombsBefore = nestReduction.CombsBefore;
                            }
                            beehive.NestCombs = nestCombs;
                            _context.Entry(beefamily).State = EntityState.Modified;
                        }
                    }
                }
            }

            await _context.SaveChangesAsync();

            return _mapper.Map<NestReductionReadDTO>(nestReduction);
        }
    }
}
