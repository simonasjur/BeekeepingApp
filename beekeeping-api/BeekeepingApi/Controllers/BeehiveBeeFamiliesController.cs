using AutoMapper;
using BeekeepingApi.DTOs.BeehiveBeeFamilyDTOs;
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
    public class BeehiveBeeFamiliesController : ControllerBase
    {
        private readonly BeekeepingContext _context;
        private readonly IMapper _mapper;

        public BeehiveBeeFamiliesController(BeekeepingContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/beehiveBeeFamilies/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<BeehiveBeeFamilyReadDTO>> GetOneBeehiveBeeFamily(long id)
        {
            var beehiveBeeFamily = await _context.BeehiveBeeFamilies.FindAsync(id);
            if (beehiveBeeFamily == null)
                return NotFound();

            var beehive = await _context.Beehives.FindAsync(beehiveBeeFamily.BeehiveId);
            var currentUserId = long.Parse(User.Identity.Name);
            var farmWorker = await _context.FarmWorkers.FindAsync(currentUserId, beehive.FarmId);
            if (farmWorker == null)
            {
                return Forbid();
            }

            return _mapper.Map<BeehiveBeeFamilyReadDTO>(beehiveBeeFamily);
        }

        // GET: api/beefamilies/{id}/beehiveBeefamilies
        [HttpGet("/api/beefamilies/{id}/beehiveBeefamilies")]
        [EnableQuery()]
        public async Task<ActionResult<IEnumerable<BeehiveBeeFamilyReadDTO>>> GetBeefamilyBeehiveBeefamilies(long id)
        {
            var beefamily = await _context.BeeFamilies.FindAsync(id);
            if (beefamily == null)
                return NotFound();

            var currentUserId = long.Parse(User.Identity.Name);
            var farmWorker = await _context.FarmWorkers.FindAsync(currentUserId, beefamily.FarmId);
            if (farmWorker == null)
            {
                return Forbid();
            }

            var beefamilyBeehives = await _context.BeehiveBeeFamilies.Where(l => l.BeeFamilyId == id).ToListAsync();
            foreach (BeehiveBeeFamily beefamilyBeehive in beefamilyBeehives)
            {
                await _context.Entry(beefamilyBeehive).Reference(ab => ab.Beehive).LoadAsync();
            }

            return _mapper.Map<IEnumerable<BeehiveBeeFamilyReadDTO>>(beefamilyBeehives).ToList();
        }

        //POST: api/beehiveBeeFamilies
        [HttpPost]
        public async Task<ActionResult<BeehiveBeeFamilyReadDTO>> CreateBeehiveBeeFamily(BeehiveBeeFamilyCreateDTO beehiveBeeFamilyCreateDTO)
        {
            var beehiveBeeFamilyDuplicate = _context.BeehiveBeeFamilies.FirstOrDefault(bb =>
                (bb.BeeFamilyId == beehiveBeeFamilyCreateDTO.BeeFamilyId && bb.DepartDate == null) ||
                (bb.BeehiveId == beehiveBeeFamilyCreateDTO.BeehiveId && bb.DepartDate == null));
            if (beehiveBeeFamilyDuplicate != null)
            {
                return BadRequest();
            }

            var beehive = await _context.Beehives.FindAsync(beehiveBeeFamilyCreateDTO.BeehiveId);
            if (beehive == null)
            {
                return BadRequest();
            }

            var beeFamily = await _context.BeeFamilies.FindAsync(beehiveBeeFamilyCreateDTO.BeeFamilyId);
            if (beeFamily == null || beehive.FarmId != beeFamily.FarmId)
            {
                return BadRequest();
            }

            var currentUserId = long.Parse(User.Identity.Name);
            var farmWorker = await _context.FarmWorkers.FindAsync(currentUserId, beehive.FarmId);
            if (farmWorker == null || farmWorker.Permissions[0] != '1')
            {
                return Forbid();
            }

            var beehiveBeeFamily = _mapper.Map<BeehiveBeeFamily>(beehiveBeeFamilyCreateDTO);
            _context.BeehiveBeeFamilies.Add(beehiveBeeFamily);
            await _context.SaveChangesAsync();

            var beehiveBeeFamilyReadDTO = _mapper.Map<BeehiveBeeFamilyReadDTO>(beehiveBeeFamily);
            return CreatedAtAction(nameof(GetOneBeehiveBeeFamily), new { id = beehiveBeeFamily.Id }, beehiveBeeFamilyReadDTO);
        }

        //PUT: api/beehiveBeeFamilies/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBeehiveBeeFamily(long id, BeehiveBeeFamilyEditDTO beehiveBeeFamilyEditDTO)
        {
            if (id != beehiveBeeFamilyEditDTO.Id)
            {
                return BadRequest();
            }

            var beehiveBeeFamily = await _context.BeehiveBeeFamilies.FindAsync(id);
            if (beehiveBeeFamily == null)
            {
                return BadRequest();
            }

            //If apiaryBeeFamily DepartDate have value, that value can't be null
            if (beehiveBeeFamily.DepartDate != null && beehiveBeeFamilyEditDTO.DepartDate == null)
            {
                return BadRequest();
            }

            var beehive = await _context.Beehives.FindAsync(beehiveBeeFamily.BeehiveId);
            var currentUserId = long.Parse(User.Identity.Name);
            var farmWorker = await _context.FarmWorkers.FindAsync(currentUserId, beehive.FarmId);
            if (farmWorker == null || farmWorker.Permissions[1] != '1')
            {
                return Forbid();
            }

            _mapper.Map(beehiveBeeFamilyEditDTO, beehiveBeeFamily);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        //DELETE: api/beehiveBeeFamilies/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult<BeehiveBeeFamilyReadDTO>> DeleteBeehiveBeeFamily(long id)
        {
            var beehiveBeeFamily = await _context.BeehiveBeeFamilies.FindAsync(id);
            if (beehiveBeeFamily == null)
            {
                return BadRequest();
            }

            var beehive = await _context.Beehives.FindAsync(beehiveBeeFamily.BeehiveId);
            var currentUserId = long.Parse(User.Identity.Name);
            var farmWorker = await _context.FarmWorkers.FindAsync(currentUserId, beehive.FarmId);
            if (farmWorker == null || farmWorker.Permissions[2] != '1')
            {
                return Forbid();
            }

            _context.BeehiveBeeFamilies.Remove(beehiveBeeFamily);
            await _context.SaveChangesAsync();

            return _mapper.Map<BeehiveBeeFamilyReadDTO>(beehiveBeeFamily);
        }
    }
}
