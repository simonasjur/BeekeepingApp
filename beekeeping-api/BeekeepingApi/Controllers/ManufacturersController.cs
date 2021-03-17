//JJ
using AutoMapper;
using BeekeepingApi.DTOs.ManufacturerDTOs;
using BeekeepingApi.Models;
using Microsoft.AspNetCore.Http;
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
    public class ManufacturersController : ControllerBase
    {
        private readonly BeekeepingContext _context;
        private readonly IMapper _mapper;

        public ManufacturersController(BeekeepingContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        //GET: api/manufacturers/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ManufacturerReadDTO>> GetManufacturer(long id)
        {
            var manufacturer = await _context.Manufacturers.FindAsync(id);
            if (manufacturer == null)
            {
                return NotFound();
            }

            return _mapper.Map<ManufacturerReadDTO>(manufacturer);
        }

        //GET: api/manufacturers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ManufacturerReadDTO>>> GetAllManufacturers()
        {
            var manufacturers = await _context.Manufacturers.ToListAsync();
            return _mapper.Map<IEnumerable<ManufacturerReadDTO>>(manufacturers).ToList();
        }

        //POST: api/manufacturers
        [HttpPost]
        public async Task<ActionResult<ManufacturerReadDTO>> CreateManufacturer(Manufacturer manufacturer)
        {
            _context.Manufacturers.Add(manufacturer);
            await _context.SaveChangesAsync();

            var manufacturerReadDTO = _mapper.Map<ManufacturerReadDTO>(manufacturer);
            return CreatedAtAction(nameof(GetManufacturer), new { id = manufacturer.Id }, manufacturerReadDTO);
        }

        //PUT: api/manufacturers/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutManufacturer(long id, Manufacturer manufacturer)
        {
            if (id != manufacturer.Id)
            {
                return BadRequest();
            }

            _context.Entry(manufacturer).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) 
            {
                if (!ManufacturerExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        //DELETE: api/manufacturers/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult<ManufacturerReadDTO>> DeleteManufacturer(long id)
        {
            var manufacturer = await _context.Manufacturers.FindAsync(id);
            if (manufacturer == null)
            {
                return NotFound();
            }

            _context.Manufacturers.Remove(manufacturer);
            await _context.SaveChangesAsync();

            return _mapper.Map<ManufacturerReadDTO>(manufacturer);
        }

        private bool ManufacturerExists(long id)
        {
            return _context.Manufacturers.Any(e => e.Id == id);
        }
    }
}
