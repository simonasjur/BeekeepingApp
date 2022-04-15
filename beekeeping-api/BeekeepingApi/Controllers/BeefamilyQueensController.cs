using AutoMapper;
using BeekeepingApi.DTOs.BeeFamilyQueenDTOs;
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
    public class BeefamilyQueensController : ControllerBase
    {
        private readonly BeekeepingContext _context;
        private readonly IMapper _mapper;

        public BeefamilyQueensController(BeekeepingContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/beefamilyQueens/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<BeeFamilyQueenReadDTO>> GetBeeFamilyQueen(long id)
        {
            var beefamilyQueen = await _context.BeeFamilyQueens.FindAsync(id);
            if (beefamilyQueen == null)
            {
                return NotFound();
            }

            var beefamily = await _context.BeeFamilies.FindAsync(beefamilyQueen.BeeFamilyId);
            var currentUserId = long.Parse(User.Identity.Name);
            var farmWorker = await _context.FarmWorkers.FindAsync(currentUserId, beefamily.FarmId);
            if (farmWorker == null)
            {
                return Forbid();
            }

            await _context.Entry(beefamilyQueen).Reference(fq => fq.Queen).LoadAsync();
            return _mapper.Map<BeeFamilyQueenReadDTO>(beefamilyQueen);
        }

        //GET: api/beefamilies/{beefamilyId}/beefamilyQueens
        [HttpGet("/api/beefamilies/{beefamilyId}/beefamilyQueens")]
        [EnableQuery()]
        public async Task<ActionResult<IEnumerable<BeeFamilyQueenReadDTO>>> GetBeefamilyQueens(long beeFamilyId)
        {
            var beefamily = await _context.BeeFamilies.FindAsync(beeFamilyId);
            if (beefamily == null)
            {
                return BadRequest();
            }

            var currentUserId = long.Parse(User.Identity.Name);
            var farmWorker = await _context.FarmWorkers.FindAsync(currentUserId, beefamily.FarmId);
            if (farmWorker == null)
            {
                return Forbid();
            }

            var queensList = await _context.BeeFamilyQueens.Where(fq => fq.BeeFamilyId == beeFamilyId).ToListAsync();
            foreach (BeefamilyQueen beefamilyQueen in queensList)
            {
                await _context.Entry(beefamilyQueen).Reference(fq => fq.Queen).LoadAsync();
            }

            return _mapper.Map<IEnumerable<BeeFamilyQueenReadDTO>>(queensList).ToList();
        }

        //POST: api/beefamilyQueens
        [HttpPost]
        public async Task<ActionResult<BeeFamilyQueenReadDTO>> CreateBeefamilyQueen(BeeFamilyQueenCreateDTO beefamilyQueenCreateDTO)
        {
            var beefamilyQueenDuplicate = _context.BeeFamilyQueens.FirstOrDefault(fq =>
                (fq.BeeFamilyId == beefamilyQueenCreateDTO.BeeFamilyId && fq.TakeOutDate == null) ||
                (fq.QueenId == beefamilyQueenCreateDTO.QueenId && fq.TakeOutDate == null));
            if (beefamilyQueenDuplicate != null)
            {
                return BadRequest();
            }

            var queen = await _context.Queens.FindAsync(beefamilyQueenCreateDTO.QueenId);
            if (queen == null)
            {
                return BadRequest();
            }

            var beeFamily = await _context.BeeFamilies.FindAsync(beefamilyQueenCreateDTO.BeeFamilyId);
            if (beeFamily == null || queen.FarmId != beeFamily.FarmId)
            {
                return BadRequest();
            }

            var currentUserId = long.Parse(User.Identity.Name);
            var farmWorker = await _context.FarmWorkers.FindAsync(currentUserId, queen.FarmId);
            if (farmWorker == null)
            {
                return Forbid();
            }

            var beefamilyQueen = _mapper.Map<BeefamilyQueen>(beefamilyQueenCreateDTO);
            _context.BeeFamilyQueens.Add(beefamilyQueen);
            await _context.SaveChangesAsync();

            var beefamilyQueenReadDTO = _mapper.Map<BeeFamilyQueenReadDTO>(beefamilyQueen);
            return CreatedAtAction(nameof(GetBeeFamilyQueen), new { id = beefamilyQueen.Id }, beefamilyQueenReadDTO);
        }

        //PUT: api/beefamilyQueens/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBeefamilyQueen(long id, BeeFamilyQueenEditDTO beefamilyQueenEditDTO)
        {
            if (id != beefamilyQueenEditDTO.Id)
            {
                return BadRequest();
            }

            var beefamilyQueen = await _context.BeeFamilyQueens.FindAsync(id);
            if (beefamilyQueen == null)
            {
                return BadRequest();
            }

            if (beefamilyQueenEditDTO.TakeOutDate != null)
            {
                var newInsertDate = beefamilyQueenEditDTO.InsertDate ?? DateTime.Today;
                var newTakeOutDate = beefamilyQueenEditDTO.TakeOutDate ?? DateTime.Today;
                if (DateTime.Compare(newInsertDate, newTakeOutDate) > 0)
                {
                    return BadRequest("Take out date is earlier than insert date");
                }
            }
            
            if ((beefamilyQueen.TakeOutDate != null && beefamilyQueenEditDTO.TakeOutDate == null))
            {
                return BadRequest("Take out date can't be deleted");
            }

            var beefamily = await _context.BeeFamilies.FindAsync(beefamilyQueen.BeeFamilyId);
            var currentUserId = long.Parse(User.Identity.Name);
            var farmWorker = await _context.FarmWorkers.FindAsync(currentUserId, beefamily.FarmId);
            if (farmWorker == null)
            {
                return Forbid();
            }

            _mapper.Map(beefamilyQueenEditDTO, beefamilyQueen);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        //DELETE: api/beefamilyQueens/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult<BeeFamilyQueenReadDTO>> DeleteBeefamilyQueen(long id)
        {
            var beefamilyQueen = await _context.BeeFamilyQueens.FindAsync(id);
            if (beefamilyQueen == null)
            {
                return BadRequest();
            }

            var beefamily = await _context.BeeFamilies.FindAsync(beefamilyQueen.BeeFamilyId);
            var currentUserId = long.Parse(User.Identity.Name);
            var farmWorker = await _context.FarmWorkers.FindAsync(currentUserId, beefamily.FarmId);
            if (farmWorker == null)
            {
                return Forbid();
            }

            _context.BeeFamilyQueens.Remove(beefamilyQueen);
            await _context.SaveChangesAsync();

            return _mapper.Map<BeeFamilyQueenReadDTO>(beefamilyQueen);
        }
    }
}
