using AutoMapper;
using BeekeepingApi.DTOs.BeeFamilyDTO;
using BeekeepingApi.DTOs.BeehiveDTOs;
using BeekeepingApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeekeepingApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BeeFamiliesController : ControllerBase
    {
        private readonly BeekeepingContext _context;
        private readonly IMapper _mapper;

        public BeeFamiliesController(BeekeepingContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        //GET: api/beeFamilies/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<BeeFamilyReadDTO>> GetBeeFamily(long id)
        {
            var beeFamily = await _context.BeeFamilies.FindAsync(id);
            if (beeFamily == null)
            {
                return NotFound();
            }

            var currentUserId = long.Parse(User.Identity.Name);
            var farmWorker = await _context.FarmWorkers.FindAsync(currentUserId, beeFamily.FarmId);
            if (farmWorker == null)
            {
                return Forbid();
            }

            //Adds beehive to bee family
            /*BeehiveReadDTO beehiveReadDTO = null;
            var beehiveBeeFamily = await _context.BeehiveBeeFamilies.FirstOrDefaultAsync(bf => bf.BeeFamilyId == beeFamily.Id &&
                                                                                               bf.DepartDate == null);
            if (beehiveBeeFamily != null)
            {
                var beehive = await _context.Beehives.FirstOrDefaultAsync(b => b.Id == beehiveBeeFamily.BeehiveId);
                beehiveReadDTO = _mapper.Map<BeehiveReadDTO>(beehive);
            }
            var beeFamilyReadDTO = _mapper.Map<BeeFamilyReadDTO>(beeFamily);
            beeFamilyReadDTO.Beehive = beehiveReadDTO;*/

            return _mapper.Map<BeeFamilyReadDTO>(beeFamily);
        }

        //GET: api/farms/{farmId}/beeFamilies
        [HttpGet("/api/farms/{farmId}/beeFamilies")]
        public async Task<ActionResult<IEnumerable<BeeFamilyReadDTO>>> GetFarmBeeFamilies(long farmId)
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

            var beeFamilies = await _context.BeeFamilies.Where(b => b.FarmId == farmId).ToListAsync();

            //Generates bee families list with beehive data
            /*List<BeeFamilyReadDTO> beeFamilyReadDTOs = new List<BeeFamilyReadDTO>();
            foreach (BeeFamily family in beeFamilies)
            {
                BeehiveReadDTO beehiveReadDTO = null;
                var beehiveBeeFamily = await _context.BeehiveBeeFamilies.FirstOrDefaultAsync(bf => bf.BeeFamilyId == family.Id &&
                                                                                                   bf.DepartDate == null);
                if (beehiveBeeFamily != null)
                {
                    var beehive = await _context.Beehives.FirstOrDefaultAsync(b => b.Id == beehiveBeeFamily.BeehiveId);
                    beehiveReadDTO = _mapper.Map<BeehiveReadDTO>(beehive);
                }
                var beeFamilyReadDTO = _mapper.Map<BeeFamilyReadDTO>(family);
                beeFamilyReadDTO.Beehive = beehiveReadDTO;
                beeFamilyReadDTOs.Add(beeFamilyReadDTO);
            }

            return beeFamilyReadDTOs;*/
            return _mapper.Map<IEnumerable<BeeFamilyReadDTO>>(beeFamilies).ToList();
        }

        //POST: api/beeFamiles
        [HttpPost]
        public async Task<ActionResult<BeeFamilyReadDTO>> CreateBeeFamily(BeeFamilyCreateDTO beeFamilyCreateDTO)
        {
            var farm = await _context.Farms.FindAsync(beeFamilyCreateDTO.FarmId);
            if (farm == null)
            {
                return BadRequest("nera farmos");
            }

            var currentUserId = long.Parse(User.Identity.Name);
            var farmWorker = await _context.FarmWorkers.FindAsync(currentUserId, farm.Id);
            if (farmWorker == null)
            {
                return Forbid();
            }

            if (beeFamilyCreateDTO.Origin == null || !IsOriginCorrect(beeFamilyCreateDTO.Origin ?? 0))
            {
                return BadRequest("Incorrect Origin");
            }

            var beehive = await _context.Beehives.FindAsync(beeFamilyCreateDTO.BeehiveId);
            var apiary = await _context.Apiaries.FindAsync(beeFamilyCreateDTO.ApiaryId);
            if (beehive == null || apiary == null)
            {
                return BadRequest("nera avilio arba bityno");
            }

            if (beehive.Type == BeehiveTypes.Dadano && beeFamilyCreateDTO.NestCombs > beehive.MaxNestCombs)
            {
                return BadRequest("Nest combs count is higher than beehive maximum nest combs value");
            }

            //Creates bee family
            var beeFamily = _mapper.Map<BeeFamily>(beeFamilyCreateDTO);
            beeFamily.IsNucleus = false;
            beeFamily.State = BeeFamilyStates.Gyvena;
            _context.BeeFamilies.Add(beeFamily);
            await _context.SaveChangesAsync();

            //Creates apiary bee family
            ApiaryBeeFamily apiaryBeeFamily = new ApiaryBeeFamily()
            {
                ApiaryId = apiary.Id,
                BeeFamilyId = beeFamily.Id,
                ArriveDate = beeFamilyCreateDTO.ArriveDate
            };
            _context.ApiaryBeeFamilies.Add(apiaryBeeFamily);
            await _context.SaveChangesAsync();

            //Creates beehive bee family
            BeehiveBeeFamily beehiveBeeFamily = new BeehiveBeeFamily()
            {
                BeehiveId = beehive.Id,
                BeeFamilyId = beeFamily.Id,
                ArriveDate = beeFamilyCreateDTO.ArriveDate
            };
            _context.BeehiveBeeFamilies.Add(beehiveBeeFamily);
            await _context.SaveChangesAsync();

            //Updates beehive
            beehive.IsEmpty = false;
            if (beehive.Type == BeehiveTypes.Dadano)
            {
                beehive.NestCombs = beeFamilyCreateDTO.NestCombs;
            }
            _context.Entry(beehive).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            //Adds supers
            if (beehive.Type == BeehiveTypes.Daugiaaukštis)
            {
                for (int i = 0; i < beeFamilyCreateDTO.SupersCount; i++)
                {
                    BeehiveComponent super = new BeehiveComponent()
                    {
                        Type = ComponentTypes.Aukštas,
                        Position = i + 1,
                        InstallationDate = beeFamilyCreateDTO.ArriveDate,
                        BeehiveId = beehive.Id
                    };
                    _context.BeehiveComponents.Add(super);
                }
                await _context.SaveChangesAsync();
            }

            var beeFamilyReadDTO = _mapper.Map<BeeFamilyReadDTO>(beeFamily);
            return CreatedAtAction(nameof(GetBeeFamily), new { id = beeFamily.Id }, beeFamilyReadDTO);
        }

        // PUT: api/beeFamilies/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBeeFamily(long id, BeeFamilyEditDTO beeFamilyEditDTO)
        {
            if (id != beeFamilyEditDTO.Id)
            {
                return BadRequest();
            }

            var beeFamily = await _context.BeeFamilies.FindAsync(id);
            if (beeFamily == null)
            {
                return NotFound();
            }

            var currentUserId = long.Parse(User.Identity.Name);
            var farmWorker = await _context.FarmWorkers.FindAsync(currentUserId, beeFamily.FarmId);
            if (farmWorker == null)
            {
                return Forbid();
            }

            if (beeFamilyEditDTO.Origin == null || !IsOriginCorrect(beeFamilyEditDTO.Origin ?? 0))
            {
                return BadRequest("Incorrect Origin");
            }

            if (beeFamilyEditDTO.State == null || !IsStateCorrect(beeFamilyEditDTO.State ?? 0))
            {
                return BadRequest("Incorrect State");
            }

            _mapper.Map(beeFamilyEditDTO, beeFamily);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        //DELETE: api/beeFamilies/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult<BeeFamilyReadDTO>> DeleteBeeFamily(long id)
        {
            var beeFamily = await _context.BeeFamilies.FindAsync(id);
            if (beeFamily == null)
            {
                return NotFound();
            }

            var currentUserId = long.Parse(User.Identity.Name);
            var farmWorker = await _context.FarmWorkers.FindAsync(currentUserId, beeFamily.FarmId);
            if (farmWorker == null)
            {
                return Forbid();
            }

            _context.BeeFamilies.Remove(beeFamily);
            await _context.SaveChangesAsync();

            return _mapper.Map<BeeFamilyReadDTO>(beeFamily);
        }

        private bool IsOriginCorrect(BeeFamilyOrigins origin)
        {
            return origin == BeeFamilyOrigins.Spiečius || origin == BeeFamilyOrigins.IšKitosŠeimos ||
                   origin == BeeFamilyOrigins.Pirkta || origin == BeeFamilyOrigins.Padovanota;
        }

        private bool IsStateCorrect(BeeFamilyStates state)
        {
            return state == BeeFamilyStates.Gyvena || state == BeeFamilyStates.Išsispietus ||
                   state == BeeFamilyStates.SujungtaSuKitaŠeima || state == BeeFamilyStates.Išmirusi;
        }
    }
}