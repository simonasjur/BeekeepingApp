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

            var supers = await _context.Supers.Where(s => s.BeehiveId == beehiveId)
                                              .OrderByDescending(s => s.Position)
                                              .ToListAsync();

            return _mapper.Map<IEnumerable<SuperReadDTO>>(supers).ToList();
        }

        //POST: api/supers
        [HttpPost]
        public async Task<ActionResult<SuperReadDTO>> CreateSuper(SuperCreateDTO superCreateDTO)
        {
            var beehive = await _context.Beehives.FindAsync(superCreateDTO.BeehiveId);
            if (beehive == null || beehive.Type == BeehiveTypes.Dadano)
            {
                return BadRequest();
            }

            var currentUserId = long.Parse(User.Identity.Name);
            var farmWorker = await _context.FarmWorkers.FindAsync(currentUserId, beehive.FarmId);
            if (farmWorker == null)
            {
                return Forbid();
            }

            //Checks if new super position is correct
            var beehiveSupers = await _context.Supers.Where(s => s.BeehiveId == beehive.Id).OrderBy(s => s.Position).ToArrayAsync();
            var lastBeehiveSuper = beehiveSupers.LastOrDefault();
            if (lastBeehiveSuper != null)
            {
                if (lastBeehiveSuper.Position + 1 >= superCreateDTO.Position)
                {
                    //If new super is inserted in beehive, then all above supers positions increased
                    for (int i = superCreateDTO.Position - 1; i < beehiveSupers.Length; i++)
                    {
                        beehiveSupers[i].Position++;
                        _context.Entry(beehiveSupers[i]).State = EntityState.Modified;
                    }
                } else
                {
                    return BadRequest("Incorrect super position");
                }
            } else if (superCreateDTO.Position != 1)
            {
                return BadRequest("Incorrect super position");
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

            //If super position changed, then all others beehive supers positions changed too
            if (superEditDTO.Position != super.Position)
            {
                var beehiveSupers = await _context.Supers.Where(s => s.BeehiveId == beehive.Id).OrderBy(s => s.Position).ToArrayAsync();
                var lastBeehiveSuper = beehiveSupers.LastOrDefault();
                if (superEditDTO.Position > lastBeehiveSuper.Position)
                {
                    return BadRequest("Incorrect super position");
                }

                if (superEditDTO.Position > super.Position)
                {
                    for (int i = super.Position; i < superEditDTO.Position; i++)
                    {
                        beehiveSupers[i].Position--;
                        _context.Entry(beehiveSupers[i]).State = EntityState.Modified;
                    }
                } else
                {
                    for (int i = superEditDTO.Position - 1; i < super.Position - 1; i++)
                    {
                        beehiveSupers[i].Position++;
                        _context.Entry(beehiveSupers[i]).State = EntityState.Modified;
                    }
                }
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

            var beehiveSupers = await _context.Supers.Where(s => s.BeehiveId == beehive.Id).OrderBy(s => s.Position).ToArrayAsync();
            var lastBeehiveSuper = beehiveSupers.LastOrDefault();
            if (lastBeehiveSuper.Id != super.Id)
            {
                //If deleted super is not on top on beehive, then all above supers positions decreased
                for (int i = super.Position; i < beehiveSupers.Length; i++)
                {
                    beehiveSupers[i].Position--;
                    _context.Entry(beehiveSupers[i]).State = EntityState.Modified;
                }
            }

            _context.Supers.Remove(super);
            await _context.SaveChangesAsync();

            return _mapper.Map<SuperReadDTO>(super);
        }
    }
}
