using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BeekeepingApi.Models;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using Microsoft.AspNet.OData;
using BeekeepingApi.DTOs.TodoItemDTOs;

namespace BeekeepingApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TodoItemsController : ControllerBase
    {
        private readonly BeekeepingContext _context;
        private readonly IMapper _mapper;

        public TodoItemsController(BeekeepingContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Farms/1/TodoItems
        [HttpGet("/api/Farms/{farmId}/TodoItems")]
        [EnableQuery()]
        public async Task<ActionResult<IEnumerable<TodoItemReadDTO>>> GetFarmTodoItems(long farmId)
        {
            var farm = await _context.Farms.FindAsync(farmId);
            if (farm == null)
                return NotFound();

            var currentUserId = long.Parse(User.Identity.Name);
            var farmWorker = await _context.FarmWorkers.FindAsync(currentUserId, farmId);
            if (farmWorker == null)
                return Forbid();

            var todoItemList = await _context.TodoItems.Where(l => l.FarmId == farmId).ToListAsync();

            return _mapper.Map<IEnumerable<TodoItemReadDTO>>(todoItemList).ToList();
        }

        // GET: api/Apiaries/1/TodoItems
        [HttpGet("/api/Apiaries/{apiaryId}/TodoItems")]
        [EnableQuery()]
        public async Task<ActionResult<IEnumerable<TodoItemReadDTO>>> GetApiaryTodoItems(long apiaryId)
        {
            var apiary = await _context.Apiaries.FindAsync(apiaryId);
            if (apiary == null)
                return NotFound();

            var farm = await _context.Farms.FindAsync(apiary.FarmId);
            if (farm == null)
                return NotFound();

            var currentUserId = long.Parse(User.Identity.Name);
            var farmWorker = await _context.FarmWorkers.FindAsync(currentUserId, farm.Id);
            if (farmWorker == null)
                return Forbid();

            var todoItemList = await _context.TodoItems.Where(l => l.ApiaryId == apiaryId).ToListAsync();

            return _mapper.Map<IEnumerable<TodoItemReadDTO>>(todoItemList).ToList();
        }

        // GET: api/Beehives/1/TodoItems
        [HttpGet("/api/Beehives/{beehiveId}/TodoItems")]
        [EnableQuery()]
        public async Task<ActionResult<IEnumerable<TodoItemReadDTO>>> GetBeehiveTodoItems(long beehiveId)
        {
            var beehive = await _context.Apiaries.FindAsync(beehiveId);
            if (beehive == null)
                return NotFound();

            var farm = await _context.Farms.FindAsync(beehive.FarmId);
            if (farm == null)
                return NotFound();

            var currentUserId = long.Parse(User.Identity.Name);
            var farmWorker = await _context.FarmWorkers.FindAsync(currentUserId, farm.Id);
            if (farmWorker == null)
                return Forbid();

            var todoItemList = await _context.TodoItems.Where(l => l.BeehiveId == beehiveId).ToListAsync();

            return _mapper.Map<IEnumerable<TodoItemReadDTO>>(todoItemList).ToList();
        }

        // GET: api/TodoItems/1
        [HttpGet("{id}")]
        public async Task<ActionResult<TodoItemReadDTO>> GetTodoItem(long id)
        {
            var todoItem = await _context.TodoItems.FindAsync(id);
            if (todoItem == null)
                return NotFound();

            var farm = await _context.Farms.FindAsync(todoItem.FarmId);

            var currentUserId = long.Parse(User.Identity.Name);
            var farmWorker = await _context.FarmWorkers.FindAsync(currentUserId, farm.Id);
            if (farmWorker == null)
                return Forbid();

            return _mapper.Map<TodoItemReadDTO>(todoItem);
        }

        // POST: api/TodoItems
        [HttpPost]
        public async Task<ActionResult<TodoItemReadDTO>> CreateTodoItem(TodoItemCreateDTO todoItemCreateDTO)
        {
            var farm = await _context.Farms.FindAsync(todoItemCreateDTO.FarmId);
            if (farm == null)
                return BadRequest();

            if (todoItemCreateDTO.ApiaryId != null && todoItemCreateDTO.BeehiveId == null)
            {
                var apiary = await _context.Apiaries.FindAsync(todoItemCreateDTO.ApiaryId);
                if (apiary == null || apiary.FarmId != farm.Id)
                    return BadRequest();                
            }
            else if (todoItemCreateDTO.BeehiveId != null && todoItemCreateDTO.ApiaryId == null)
            {
                var beehive = await _context.Beehives.FindAsync(todoItemCreateDTO.BeehiveId);
                if (beehive == null || beehive.FarmId != farm.Id)
                    return BadRequest();
            }
            else if (todoItemCreateDTO.BeehiveId != null && todoItemCreateDTO.ApiaryId != null)
            {
                return BadRequest();
            }

            var currentUserId = long.Parse(User.Identity.Name);
            var farmWorker = await _context.FarmWorkers.FindAsync(currentUserId, farm.Id);
            if (farmWorker == null)
                return Forbid();

            var todoItem = _mapper.Map<TodoItem>(todoItemCreateDTO);
            _context.TodoItems.Add(todoItem);
            await _context.SaveChangesAsync();

            var todoItemReadDTO = _mapper.Map<TodoItemReadDTO>(todoItem);

            return CreatedAtAction("GetTodoItem", "TodoItems", new { id = todoItem.Id }, todoItemReadDTO);
        }

        // PUT: api/TodoItems/1
        [HttpPut("{id}")]
        public async Task<IActionResult> EditTodoItem(long id, TodoItemEditDTO todoItemEditDTO)
        {
            if (id != todoItemEditDTO.Id)
                return BadRequest();

            var todoItem = await _context.TodoItems.FindAsync(id);
            if (todoItem == null)
                return NotFound();
            var farm = await _context.Farms.FindAsync(todoItem.FarmId);
            if (farm == null)
                return NotFound();

            var currentUserId = long.Parse(User.Identity.Name);
            var farmWorker = await _context.FarmWorkers.FindAsync(currentUserId, farm.Id);
            if (farmWorker == null)
                return Forbid();

            _mapper.Map(todoItemEditDTO, todoItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/TodoItems/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<TodoItemReadDTO>> DeleteTodoItem(long id)
        {
            var todoItem = await _context.TodoItems.FindAsync(id);
            if (todoItem == null)
                return NotFound();
            var farm = await _context.Farms.FindAsync(todoItem.FarmId);
            if (farm == null)
                return NotFound();

            var currentUserId = long.Parse(User.Identity.Name);
            var farmWorker = await _context.FarmWorkers.FindAsync(currentUserId, farm.Id);
            if (farmWorker == null)
                return Forbid();

            _context.TodoItems.Remove(todoItem);
            await _context.SaveChangesAsync();

            return _mapper.Map<TodoItemReadDTO>(todoItem);
        }
    }
}
