using AutoMapper;
using BeekeepingApi.DTOs.FoodDTOs;
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
    public class FoodsController : ControllerBase
    {
        private readonly BeekeepingContext _context;
        private readonly IMapper _mapper;

        public FoodsController(BeekeepingContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Farms/1/Foods
        [HttpGet("/api/Farms/{farmId}/Foods")]
        public async Task<ActionResult<IEnumerable<FoodReadDTO>>> GetFarmFoods(long farmId)
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

            var foods = await _context.Foods.Where(f => f.FarmId == farmId).ToListAsync();

            return _mapper.Map<IEnumerable<FoodReadDTO>>(foods).ToList();
        }

        // GET: api/Foods/1
        [HttpGet("{id}")]
        public async Task<ActionResult<FoodReadDTO>> GetFood(long id)
        {
            var food = await _context.Foods.FindAsync(id);
            if (food == null)
            {
                return NotFound();
            }

            var currentUserId = long.Parse(User.Identity.Name);
            var farmWorker = await _context.FarmWorkers.FindAsync(currentUserId, food.FarmId);
            if (farmWorker == null)
            {
                return Forbid();
            }

            return _mapper.Map<FoodReadDTO>(food);
        }

        // POST: api/Foods
        [HttpPost]
        public async Task<ActionResult<FoodReadDTO>> CreateFood(FoodCreateDTO foodCreateDTO)
        {
            var farm = await _context.Farms.FindAsync(foodCreateDTO.FarmId);
            if (farm == null)
            {
                return BadRequest();
            }

            var currentUserId = long.Parse(User.Identity.Name);
            var farmWorker = await _context.FarmWorkers.FindAsync(currentUserId, farm.Id);
            if (farmWorker == null || farmWorker.Permissions[27] != '1')
            {
                return Forbid();
            }


            var food = _mapper.Map<Food>(foodCreateDTO);
            _context.Foods.Add(food);
            await _context.SaveChangesAsync();

            var foodReadDTO = _mapper.Map<FoodReadDTO>(food);

            return CreatedAtAction("GetFood", "Foods", new { id = food.Id }, foodReadDTO);
        }

        // PUT: api/Foods/1
        [HttpPut("{id}")]
        public async Task<IActionResult> EditFood(long id, FoodEditDTO foodEditDTO)
        {
            if (id != foodEditDTO.Id)
            {
                return BadRequest();
            }

            var food = await _context.Foods.FindAsync(id);
            if (food == null)
            {
                return NotFound();
            }

            var currentUserId = long.Parse(User.Identity.Name);
            var farmWorker = await _context.FarmWorkers.FindAsync(currentUserId, food.FarmId);
            if (farmWorker == null || farmWorker.Permissions[28] != '1')
            {
                return Forbid();
            }

            _mapper.Map(foodEditDTO, food);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Foods/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<FoodReadDTO>> DeleteFood(long id)
        {
            var food = await _context.Foods.FindAsync(id);
            if (food == null)
            {
                return NotFound();
            }

            var currentUserId = long.Parse(User.Identity.Name);
            var farmWorker = await _context.FarmWorkers.FindAsync(currentUserId, food.FarmId);
            if (farmWorker == null || farmWorker.Permissions[29] != '1')
            {
                return Forbid();
            }

            var relatedFeedings = await _context.Feedings.Where(f => f.FoodId == id).ToListAsync();
            foreach (var feeding in relatedFeedings)
            {
                _context.Feedings.Remove(feeding);
            }

            _context.Foods.Remove(food);
            await _context.SaveChangesAsync();

            return _mapper.Map<FoodReadDTO>(food);
        }
    }
}
