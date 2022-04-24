using AutoMapper;
using BeekeepingApi.DTOs.QueenDTOs;
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
    public class QueensController : ControllerBase
    {
        private readonly BeekeepingContext _context;
        private readonly IMapper _mapper;

        public QueensController(BeekeepingContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Farms/1/Queens
        [HttpGet("/api/Farms/{farmId}/Queens")]
        [EnableQuery()]
        public async Task<ActionResult<IEnumerable<QueenReadDTO>>> GetFarmQueens(long farmId)
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

            var queensList = await _context.Queens.Where(q => q.FarmId == farmId).ToListAsync();

            return _mapper.Map<IEnumerable<QueenReadDTO>>(queensList).ToList();
        }

        // GET: api/Queens/1
        [HttpGet("{id}")]
        public async Task<ActionResult<QueenReadDTO>> GetQueen(long id)
        {
            var queen = await _context.Queens.FindAsync(id);
            if (queen == null)
            {
                return NotFound();
            }    

            var currentUserId = long.Parse(User.Identity.Name);
            var farmWorker = await _context.FarmWorkers.FindAsync(currentUserId, queen.FarmId);
            if (farmWorker == null)
                return Forbid();

            return _mapper.Map<QueenReadDTO>(queen);
        }

        // POST: api/Queens
        [HttpPost]
        public async Task<ActionResult<QueenReadDTO>> CreateQueen(QueenCreateDTO queenCreateDTO)
        {
            var farm = await _context.Farms.FindAsync(queenCreateDTO.FarmId);
            if (farm == null)
            {
                return BadRequest();
            }
                
            var currentUserId = long.Parse(User.Identity.Name);
            var farmWorker = await _context.FarmWorkers.FindAsync(currentUserId, farm.Id);
            if (farmWorker == null || farmWorker.Permissions[18] != '1')
            {
                return Forbid();
            }

            if (!IsQueenStateValid(queenCreateDTO.State ?? 0) || !IsCreatingState(queenCreateDTO.State ?? 0))
            {
                return BadRequest();
            }
            
            if (queenCreateDTO.State == QueenState.Lopšys)
            {
                queenCreateDTO.IsFertilized = false;
                queenCreateDTO.MarkingColor = null;
                queenCreateDTO.HatchingDate = null;
                queenCreateDTO.BroodStartDate = null;
            }

            var queen = _mapper.Map<Queen>(queenCreateDTO);
            _context.Queens.Add(queen);
            await _context.SaveChangesAsync();

            var queenReadDTO = _mapper.Map<QueenReadDTO>(queen);

            return CreatedAtAction("GetQueen", "Queens", new { id = queen.Id }, queenReadDTO);
        }

        // PUT: api/Queens/1
        [HttpPut("{id}")]
        public async Task<IActionResult> EditQueen(long id, QueenEditDTO queenEditDTO)
        {
            if (id != queenEditDTO.Id)
            {
                return BadRequest();
            }

            var queen = await _context.Queens.FindAsync(id);
            if (queen == null)
            {
                return NotFound();
            }
                
            var farm = await _context.Farms.FindAsync(queen.FarmId);
            if (farm == null)
            {
                return NotFound();
            }

            var currentUserId = long.Parse(User.Identity.Name);
            var farmWorker = await _context.FarmWorkers.FindAsync(currentUserId, farm.Id);
            if (farmWorker == null || farmWorker.Permissions[19] != '1')
            {
                return Forbid();
            }

            var newQueenState = queenEditDTO.State ?? 0;
            if ((!IsQueenStateValid(newQueenState)) ||
                (queen.State != QueenState.Lopšys && queenEditDTO.State == QueenState.Lopšys) ||
                (IsFinalState(queen.State) && !IsFinalState(newQueenState)))
            {
                return BadRequest("Invalid state");
            }

            if (queenEditDTO.State == QueenState.Lopšys)
            {
                if (!IsCellDataCorrect(_mapper.Map<Queen>(queenEditDTO)))
                {
                    return BadRequest("Cell data incorrect");
                }
            }

            _mapper.Map(queenEditDTO, queen);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Queens/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<QueenReadDTO>> DeleteQueen(long id)
        {
            var queen = await _context.Queens.FindAsync(id);
            if (queen == null)
            {
                return NotFound();
            }
                
            var farm = await _context.Farms.FindAsync(queen.FarmId);
            if (farm == null)
            {
                return NotFound();
            }
                
            var currentUserId = long.Parse(User.Identity.Name);
            var farmWorker = await _context.FarmWorkers.FindAsync(currentUserId, farm.Id);
            if (farmWorker == null || farmWorker.Permissions[20] != '1')
            {
                return Forbid();
            }

            var relatedQueensRaisings = await _context.QueensRaisings.Where(qr => qr.MotherId == id).ToListAsync();
            foreach(var raising in relatedQueensRaisings)
            {
                _context.QueensRaisings.Remove(raising);
            }
            await _context.SaveChangesAsync();

            _context.Queens.Remove(queen);
            await _context.SaveChangesAsync();

            return _mapper.Map<QueenReadDTO>(queen);
        }

        private bool IsQueenStateValid(QueenState state)
        {
            if (state == QueenState.Lopšys || state == QueenState.GyvenaAvilyje ||
                state == QueenState.PriduodamaŠeimai || state == QueenState.IzoliuotaNarvelyje ||
                state == QueenState.Parduota || state == QueenState.Išsispietusi ||
                state == QueenState.Numirusi)
            {
                return true;
            }
            return false;
        }

        private bool IsCreatingState(QueenState state)
        {
            return (state & (QueenState.Lopšys | QueenState.GyvenaAvilyje |
                             QueenState.PriduodamaŠeimai | QueenState.IzoliuotaNarvelyje)) > 0;
        }

        private bool IsFinalState(QueenState state)
        {
            return (state & (QueenState.Parduota | QueenState.Išsispietusi |
                             QueenState.Numirusi)) > 0;
        }

        private bool IsCellDataCorrect(Queen queen)
        {
            return queen.State == QueenState.Lopšys && queen.HatchingDate == null &&
                   queen.MarkingColor == null && queen.IsFertilized == false &&
                   queen.BroodStartDate == null;
        }
    }
}
