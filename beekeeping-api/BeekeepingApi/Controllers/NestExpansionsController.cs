using AutoMapper;
using BeekeepingApi.DTOs.NestExpansionDTOs;
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
    public class NestExpansionsController : ControllerBase
    {
        private readonly BeekeepingContext _context;
        private readonly IMapper _mapper;

        private const int DaysCount = 8;

        public NestExpansionsController(BeekeepingContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Beefamilies/1/NestExpansions
        [HttpGet("/api/Beefamilies/{beefamilyId}/NestExpansions")]
        [EnableQuery()]
        public async Task<ActionResult<IEnumerable<NestExpansionReadDTO>>> GetBeefamilyNestExpansions(long beefamilyId)
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

            var nestExpansions = await _context.NestExpansions.Where(ne => ne.BeefamilyId == beefamilyId).ToListAsync();

            return _mapper.Map<IEnumerable<NestExpansionReadDTO>>(nestExpansions).ToList();
        }

        // GET: api/NestExpansions/1
        [HttpGet("{id}")]
        public async Task<ActionResult<NestExpansionReadDTO>> GetNestExpansion(long id)
        {
            var nestExpansion = await _context.NestExpansions.FindAsync(id);
            if (nestExpansion == null)
            {
                return NotFound();
            }

            var beefamily = await _context.BeeFamilies.FindAsync(nestExpansion.BeefamilyId);
            var currentUserId = long.Parse(User.Identity.Name);
            var farmWorker = await _context.FarmWorkers.FindAsync(currentUserId, beefamily.FarmId);
            if (farmWorker == null)
            {
                return Forbid();
            }

            return _mapper.Map<NestExpansionReadDTO>(nestExpansion);
        }

        // POST: api/NestExpansions
        [HttpPost]
        public async Task<ActionResult<NestExpansionReadDTO>> CreateNestExpansion(NestExpansionCreateDTO nestExpansionCreateDTO)
        {
            var beefamily = await _context.BeeFamilies.FindAsync(nestExpansionCreateDTO.BeefamilyId);
            if (beefamily == null || beefamily.State == BeeFamilyStates.Išmirusi || beefamily.State == BeeFamilyStates.SujungtaSuKitaŠeima)
            {
                return BadRequest();
            }

            var currentUserId = long.Parse(User.Identity.Name);
            var farmWorker = await _context.FarmWorkers.FindAsync(currentUserId, beefamily.FarmId);
            if (farmWorker == null || farmWorker.Permissions[12] != '1')
            {
                return Forbid();
            }

            if (!IsFrameTypeValid(nestExpansionCreateDTO.FrameType ?? 0))
            {
                return BadRequest();
            }

            if (nestExpansionCreateDTO.FrameType == FrameType.NestFrame)
            {
                DateTime expansionDate = nestExpansionCreateDTO.Date ?? DateTime.Today;
                if (IsDateRecent(expansionDate))
                {
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
                    if (beehive.Type == BeehiveTypes.Dadano)
                    {
                        int newNestCombs = (beehive.NestCombs ?? 0) + (nestExpansionCreateDTO.CombSheets ?? 0) + (nestExpansionCreateDTO.Combs ?? 0);
                        if (newNestCombs > beehive.MaxNestCombs)
                        {
                            beehive.NestCombs = beehive.MaxNestCombs;
                        }
                        else
                        {
                            beehive.NestCombs = newNestCombs;
                        }
                        _context.Entry(beehive).State = EntityState.Modified;
                        await _context.SaveChangesAsync();
                    }
                }
            }

            var nestExpansion = _mapper.Map<NestExpansion>(nestExpansionCreateDTO);
            _context.NestExpansions.Add(nestExpansion);
            await _context.SaveChangesAsync();

            var nestExpansionReadDTO = _mapper.Map<NestExpansionReadDTO>(nestExpansion);

            return CreatedAtAction("GetNestExpansion", "NestExpansions", new { id = nestExpansion.Id }, nestExpansionReadDTO);
        }

        // PUT: api/NestExpansions/1
        [HttpPut("{id}")]
        public async Task<IActionResult> EditNestExpansion(long id, NestExpansionEditDTO nestExpansionEditDTO)
        {
            if (id != nestExpansionEditDTO.Id)
            {
                return BadRequest();
            }

            var nestExpansion = await _context.NestExpansions.FindAsync(id);
            if (nestExpansion == null || nestExpansion.FrameType != nestExpansionEditDTO.FrameType)
            {
                return BadRequest();
            }

            var beefamily = await _context.BeeFamilies.FindAsync(nestExpansion.BeefamilyId);
            var currentUserId = long.Parse(User.Identity.Name);
            var farmWorker = await _context.FarmWorkers.FindAsync(currentUserId, beefamily.FarmId);
            if (farmWorker == null || farmWorker.Permissions[13] != '1')
            {
                return Forbid();
            }

            if (nestExpansion.FrameType == FrameType.NestFrame &&
                beefamily.State != BeeFamilyStates.Išmirusi && beefamily.State != BeeFamilyStates.SujungtaSuKitaŠeima)
            {
                bool isOldDateRecent = IsDateRecent(nestExpansion.Date);
                bool isNewDateRecent = IsDateRecent(nestExpansionEditDTO.Date ?? DateTime.Today);
                if (isOldDateRecent || isNewDateRecent)
                {
                    var beehiveBeefamily = await _context.BeehiveBeeFamilies.FirstOrDefaultAsync(bb => bb.BeeFamilyId == beefamily.Id &&
                                                                                                       bb.DepartDate == null);
                    if (beehiveBeefamily != null)
                    {
                        var beehive = await _context.Beehives.FindAsync(beehiveBeefamily.BeehiveId);
                        if (beehive != null && beehive.Type == BeehiveTypes.Dadano)
                        {
                            int newNestCombs = beehive.NestCombs ?? 0;
                            if (!isNewDateRecent)
                            {
                                newNestCombs -= nestExpansion.Combs + nestExpansion.CombSheets;
                            } else if (!isOldDateRecent)
                            {
                                newNestCombs += (nestExpansionEditDTO.CombSheets ?? 0) + (nestExpansionEditDTO.Combs ?? 0);
                            } else
                            {
                                int oldCombs = nestExpansion.Combs + nestExpansion.CombSheets;
                                int newCombs = (nestExpansionEditDTO.CombSheets ?? 0) + (nestExpansionEditDTO.Combs ?? 0);
                                newNestCombs += newCombs - oldCombs;
                            }
                            if (newNestCombs > beehive.MaxNestCombs)
                            {
                                newNestCombs = beehive.MaxNestCombs ?? 0;
                            } else if (newNestCombs < 0) {
                                newNestCombs = 0;
                            }
                            beehive.NestCombs = newNestCombs;
                            _context.Entry(beehive).State = EntityState.Modified;
                            await _context.SaveChangesAsync();
                        }
                    }
                }
            }

            _mapper.Map(nestExpansionEditDTO, nestExpansion);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/NestExpansions/1
        [HttpDelete("{id}")]
        public async Task<ActionResult<NestExpansionReadDTO>> DeleteNestExpansion(long id)
        {
            var nestExpansion = await _context.NestExpansions.FindAsync(id);
            if (nestExpansion == null)
            {
                return NotFound();
            }

            var beefamily = await _context.BeeFamilies.FindAsync(nestExpansion.BeefamilyId);
            if (beefamily == null)
            {
                return NotFound();
            }

            var currentUserId = long.Parse(User.Identity.Name);
            var farmWorker = await _context.FarmWorkers.FindAsync(currentUserId, beefamily.FarmId);
            if (farmWorker == null || farmWorker.Permissions[14] != '1')
            {
                return Forbid();
            }

            if (nestExpansion.FrameType == FrameType.NestFrame)
            {
                DateTime expansionDate = nestExpansion.Date;
                if (IsDateRecent(expansionDate))
                {
                    var beehiveBeefamily = await _context.BeehiveBeeFamilies.FirstOrDefaultAsync(bb => bb.BeeFamilyId == beefamily.Id &&
                                                                                                       bb.DepartDate == null);
                    if (beehiveBeefamily != null)
                    {
                        var beehive = await _context.Beehives.FindAsync(beehiveBeefamily.BeehiveId);
                        if (beehive != null && beehive.Type == BeehiveTypes.Dadano)
                        {
                            int newNestCombs = (beehive.NestCombs ?? 0) - nestExpansion.CombSheets - nestExpansion.Combs;
                            if (newNestCombs > 0)
                            {
                                beehive.NestCombs = newNestCombs;
                            }
                            else
                            {
                                beehive.NestCombs = 0;
                            }
                            _context.Entry(beehive).State = EntityState.Modified;
                            await _context.SaveChangesAsync();
                        }
                    }
                }
            }

            _context.NestExpansions.Remove(nestExpansion);
            await _context.SaveChangesAsync();

            return _mapper.Map<NestExpansionReadDTO>(nestExpansion);
        }

        private bool IsFrameTypeValid(FrameType type)
        {
            return type == FrameType.NestFrame || type == FrameType.HoneyFrame;
        }

        private bool IsDateRecent(DateTime date)
        {
            DateTime today = DateTime.Today;
            return (today - date).TotalDays >= 0 && (today - date).TotalDays < DaysCount;
        }
    }
}
