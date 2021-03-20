using AutoMapper;
using BeekeepingApi.DTOs.SuperDTOs;
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

            var currentUserId = long.Parse(User.Identity.Name);
            var beehive = await _context.Beehives.FindAsync(super.BeehiveId);
            var farmWorker = await _context.FarmWorkers.FindAsync(currentUserId, beehive.FarmId);
            if (farmWorker == null)
            {
                return Forbid();
            }

            return _mapper.Map<SuperReadDTO>(super);
        }

        //POST: api/supers
        [HttpPost]
        public async Task<ActionResult<SuperReadDTO>> CreateSuper(SuperCreateDTO superCreateDTO)
        {
            var currentUserId = long.Parse(User.Identity.Name);
            var beehive = await _context.Beehives.FindAsync(superCreateDTO.BeehiveId);
            if (beehive == null)
            {
                return BadRequest();
            }

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
    }
}
