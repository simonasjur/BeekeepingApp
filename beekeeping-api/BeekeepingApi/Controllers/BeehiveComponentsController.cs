using AutoMapper;
using BeekeepingApi.DTOs.BeehiveComponentDTOs;
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
    public class BeehiveComponentsController : ControllerBase
    {
        private readonly BeekeepingContext _context;
        private readonly IMapper _mapper;

        public BeehiveComponentsController(BeekeepingContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        //GET: api/beehiveComponents/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<BeehiveComponentReadDTO>> GetBeehiveComponent(long id)
        {
            var component = await _context.BeehiveComponents.FindAsync(id);
            if (component == null)
            {
                return NotFound();
            }

            var beehive = await _context.Beehives.FindAsync(component.BeehiveId);
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

            return _mapper.Map<BeehiveComponentReadDTO>(component);
        }

        //GET: api/beehives/{beehiveId}/beehiveComponents
        [HttpGet("/api/beehives/{beehiveId}/beehiveComponents")]
        public async Task<ActionResult<IEnumerable<BeehiveComponentReadDTO>>> GetBeehiveComponents(long beehiveId)
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

            var beehiveComponents = await _context.BeehiveComponents.Where(s => s.BeehiveId == beehiveId)
                                              .OrderByDescending(s => s.Position).ThenBy(s => s.Type)
                                              .ToListAsync();

            return _mapper.Map<IEnumerable<BeehiveComponentReadDTO>>(beehiveComponents).ToList();
        }

        //POST: api/beehiveComponents
        [HttpPost]
        public async Task<ActionResult<BeehiveComponentReadDTO>> CreateBeehiveComponent(BeehiveComponentCreateDTO beehiveComponentCreateDTO)
        {
            var beehive = await _context.Beehives.FindAsync(beehiveComponentCreateDTO.BeehiveId);
            if (beehive == null)
            {
                return BadRequest();
            }

            var currentUserId = long.Parse(User.Identity.Name);
            var farmWorker = await _context.FarmWorkers.FindAsync(currentUserId, beehive.FarmId);
            if (farmWorker == null)
            {
                return Forbid();
            }

            var component = _mapper.Map<BeehiveComponent>(beehiveComponentCreateDTO);
            if (!IsComponentTypeValid(component))
            {
                return BadRequest("Invalid component Type");
            }
            if (!IsComponentDataValid(component))
            {
                return BadRequest("Invalid data");
            }

            if (beehive.Type == BeehiveTypes.Daugiaaukštis)
            {
                var beehiveSupers = await _context.BeehiveComponents.Where(s => s.BeehiveId == beehive.Id && s.Type == ComponentTypes.Aukštas)
                                                                            .OrderBy(s => s.Position).ToArrayAsync();
                var lastBeehiveSuper = beehiveSupers.LastOrDefault();

                //If component is super
                if (component.Type == ComponentTypes.Aukštas)
                {
                    //Checks if new super position is correct
                    if (lastBeehiveSuper != null)
                    {
                        if (lastBeehiveSuper.Position + 1 >= component.Position)
                        {
                            //Converts from int? to int, "if" statement checks if position have valid value, but this statement is redundant if above
                            //validation working correctly
                            int startPosition = (component.Position ?? 0) - 1;
                            if (startPosition < 0)
                            {
                                return BadRequest();
                            }
                            //If new super is inserted in beehive, then all above supers positions increased
                            for (int i = startPosition; i < beehiveSupers.Length; i++)
                            {
                                beehiveSupers[i].Position++;
                                _context.Entry(beehiveSupers[i]).State = EntityState.Modified;
                            }

                            //If inserted super is lower than queen excluder, then quen excluder position increased by 1
                            var beehiveQueenExcluder = await _context.BeehiveComponents.FirstOrDefaultAsync(s => s.BeehiveId == beehive.Id &&
                                                                                                                 s.Type == ComponentTypes.SkiriamojiTvorelė);
                            if (beehiveQueenExcluder != null && component.Position <= beehiveQueenExcluder.Position)
                            {
                                beehiveQueenExcluder.Position++;
                                _context.Entry(beehiveQueenExcluder).State = EntityState.Modified;
                            }

                            //If inserted super is lower than bee decreaser, then bee decreaser position increased by 1
                            var beehiveBeeDecreaser = await _context.BeehiveComponents.FirstOrDefaultAsync(s => s.BeehiveId == beehive.Id &&
                                                                                                                 s.Type == ComponentTypes.Išleistuvas);
                            if (beehiveBeeDecreaser != null && component.Position <= beehiveBeeDecreaser.Position)
                            {
                                beehiveBeeDecreaser.Position++;
                                _context.Entry(beehiveBeeDecreaser).State = EntityState.Modified;
                            }
                        }
                        else
                        {
                            return BadRequest("Incorrect Super position");
                        }
                    }
                    else if (component.Position != 1)
                    {
                        return BadRequest("Incorrect Super position");
                    }
                }

                if (component.Type == ComponentTypes.SkiriamojiTvorelė)
                {
                    var beehiveQueenExcluder = await _context.BeehiveComponents.FirstOrDefaultAsync(s => s.BeehiveId == beehive.Id &&
                                                                                                              s.Type == ComponentTypes.SkiriamojiTvorelė);
                    if (beehiveQueenExcluder != null)
                    {
                        return BadRequest("Queen Excluder already exist");
                    }
                    if (lastBeehiveSuper == null || component.Position >= lastBeehiveSuper.Position)
                    {
                        return BadRequest("Incorrect Queen Excluder position");
                    }
                }

                if (component.Type == ComponentTypes.DugnoSklendė)
                {
                    var bottomGate = await _context.BeehiveComponents.FirstOrDefaultAsync(s => s.BeehiveId == beehive.Id && 
                                                                                                    s.Type == ComponentTypes.DugnoSklendė);
                    if (bottomGate != null)
                    {
                        return BadRequest("Bottom Gate already exist");
                    }
                }

                if (component.Type == ComponentTypes.Išleistuvas)
                {
                    var beehiveBeeDecreaser = await _context.BeehiveComponents.FirstOrDefaultAsync(s => s.BeehiveId == beehive.Id &&
                                                                                                             s.Type == ComponentTypes.Išleistuvas);
                    if (beehiveBeeDecreaser != null)
                    {
                        return BadRequest("Bee Decreaser already exist");
                    }
                    if (lastBeehiveSuper == null || component.Position >= lastBeehiveSuper.Position)
                    {
                        return BadRequest("Incorrect Bee Decreaser position");
                    }
                }
            }

            if (beehive.Type == BeehiveTypes.Dadano)
            {
                double honeycombsSupersCount = await _context.BeehiveComponents.Where(s => s.BeehiveId == beehive.Id && s.Type == ComponentTypes.Meduvė)
                                                                            .CountAsync();
                double miniHoneycombsSupersCount = await _context.BeehiveComponents.Where(s => s.BeehiveId == beehive.Id && s.Type == ComponentTypes.Pusmeduvė)
                                                                            .CountAsync();
                double supersSpaceLeft = (beehive.MaxHoneyCombsSupers ?? 0) - honeycombsSupersCount - miniHoneycombsSupersCount / 2;
                if (component.Type == ComponentTypes.Meduvė && supersSpaceLeft < 1)
                {
                    return BadRequest("There is no space for \"Meduve\"");
                }
                if (component.Type == ComponentTypes.Pusmeduvė && supersSpaceLeft < 0.5)
                {
                    return BadRequest("There is no space for \"Pusmeduve\"");
                }
            }

            _context.BeehiveComponents.Add(component);
            await _context.SaveChangesAsync();

            var componentReadDTO = _mapper.Map<BeehiveComponentReadDTO>(component);
            return CreatedAtAction(nameof(GetBeehiveComponent), new { id = component.Id }, componentReadDTO);
        }

        //PUT: api/beehiveComponents/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBeehiveComponent(long id, BeehiveComponentEditDTO beehiveComponentEditDTO)
        {
            if (id != beehiveComponentEditDTO.Id)
            {
                return BadRequest();
            }

            var component = await _context.BeehiveComponents.FindAsync(id);
            if (component == null)
            {
                return BadRequest();
            }

            //Maybe delete BeehiveComponentEditDTO type field and this "if" is not necessary
            //Component type cannot be changed
            if (component.Type != beehiveComponentEditDTO.Type)
            {
                return BadRequest("Component type cannot be changed");
            }

            var beehive = await _context.Beehives.FindAsync(component.BeehiveId);
            var currentUserId = long.Parse(User.Identity.Name);
            var farmWorker = await _context.FarmWorkers.FindAsync(currentUserId, beehive.FarmId);
            if (farmWorker == null)
            {
                return Forbid();
            }

            var changedComponent = _mapper.Map<BeehiveComponent>(beehiveComponentEditDTO);
            changedComponent.BeehiveId = component.BeehiveId;
            if (!IsComponentDataValid(changedComponent))
            {
                return BadRequest("Incorrect data");
            }

            if (beehive.Type == BeehiveTypes.Daugiaaukštis)
            {
                var beehiveSupers = await _context.BeehiveComponents.Where(s => s.BeehiveId == beehive.Id && s.Type == ComponentTypes.Aukštas)
                                                                            .OrderBy(s => s.Position).ToArrayAsync();
                var lastBeehiveSuper = beehiveSupers.LastOrDefault();

                if (component.Type == ComponentTypes.Aukštas)
                {
                    //If super position changed, then all others beehive supers positions changed too
                    if (beehiveComponentEditDTO.Position != component.Position)
                    {
                        if (beehiveComponentEditDTO.Position > lastBeehiveSuper.Position)
                        {
                            return BadRequest("Incorrect Super position");
                        }

                        if (beehiveComponentEditDTO.Position > component.Position)
                        {
                            for (int i = component.Position ?? int.MaxValue; i < beehiveComponentEditDTO.Position; i++)
                            {
                                beehiveSupers[i].Position--;
                                _context.Entry(beehiveSupers[i]).State = EntityState.Modified;
                            }
                        }
                        else
                        {
                            for (int i = (beehiveComponentEditDTO.Position ?? int.MaxValue) - 1; i < component.Position - 1; i++)
                            {
                                beehiveSupers[i].Position++;
                                _context.Entry(beehiveSupers[i]).State = EntityState.Modified;
                            }
                        }
                    }
                }

                if (component.Type == ComponentTypes.SkiriamojiTvorelė || component.Type == ComponentTypes.Išleistuvas)
                {
                    if (lastBeehiveSuper == null || changedComponent.Position >= lastBeehiveSuper.Position)
                    {
                        return BadRequest("Incorrect position");
                    }
                }
            }
            

            _mapper.Map(beehiveComponentEditDTO, component);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        //DELETE: api/beehiveComponents/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult<BeehiveComponentReadDTO>> DeleteBeehiveComponent(long id)
        {
            var component = await _context.BeehiveComponents.FindAsync(id);
            if (component == null)
            {
                return NotFound();
            }

            var beehive = await _context.Beehives.FindAsync(component.BeehiveId);
            var currentUserId = long.Parse(User.Identity.Name);
            var farmWorker = await _context.FarmWorkers.FindAsync(currentUserId, beehive.FarmId);
            if (farmWorker == null)
            {
                return Forbid();
            }
            
            if (component.Type == ComponentTypes.Aukštas)
            {
                var beehiveSupers = await _context.BeehiveComponents.Where(s => s.BeehiveId == beehive.Id && s.Type == ComponentTypes.Aukštas)
                                                                        .OrderBy(s => s.Position).ToArrayAsync();
                var lastBeehiveSuper = beehiveSupers.LastOrDefault();

                //If deleted super is lower than queen excluder, then quen excluder position decreased by 1
                var beehiveQueenExcluder = await _context.BeehiveComponents.FirstOrDefaultAsync(s => s.BeehiveId == beehive.Id &&
                                                                                                     s.Type == ComponentTypes.SkiriamojiTvorelė);
                if (beehiveQueenExcluder != null && component.Position <= beehiveQueenExcluder.Position)
                {
                    beehiveQueenExcluder.Position--;
                    _context.Entry(beehiveQueenExcluder).State = EntityState.Modified;
                }

                //If deleted super is lower than bee decreaser, then bee decreaser position lowered by 1
                var beehiveBeeDecreaser = await _context.BeehiveComponents.FirstOrDefaultAsync(s => s.BeehiveId == beehive.Id &&
                                                                                                     s.Type == ComponentTypes.Išleistuvas);
                if (beehiveBeeDecreaser != null && component.Position <= beehiveBeeDecreaser.Position)
                {
                    beehiveBeeDecreaser.Position--;
                    _context.Entry(beehiveBeeDecreaser).State = EntityState.Modified;
                }

                if (lastBeehiveSuper.Id != component.Id)
                {
                    //If deleted super is not on top on beehive, then all above supers positions decreased
                    int startPosition = component.Position ?? int.MaxValue;
                    for (int i = startPosition; i < beehiveSupers.Length; i++)
                    {
                        beehiveSupers[i].Position--;
                        _context.Entry(beehiveSupers[i]).State = EntityState.Modified;
                    }
                }
            }

            _context.BeehiveComponents.Remove(component);
            await _context.SaveChangesAsync();

            return _mapper.Map<BeehiveComponentReadDTO>(component);
        }

        private bool IsComponentDataValid(BeehiveComponent component)
        {
            var beehive = _context.Beehives.Find(component.BeehiveId);
            if (beehive == null && component == null)
            {
                return false;
            }

            //Nukleuso sekcija can't have components
            if (beehive.Type == BeehiveTypes.NukleosoSekcija)
            {
                return false;
            }

            //Beehive only can have appropriate components depending on beehive type
            if (beehive.Type == BeehiveTypes.Dadano)
            {
                if (!IsDadanoComponent(component.Type))
                {
                    return false;
                }
            }
            else if (beehive.Type == BeehiveTypes.Daugiaaukštis)
            {
                if (!IsDaugiaaukštisComponent(component.Type))
                {
                    return false;
                }
            }

            //Some components must have position, others must not have
            if (IsComponentMustHavePosition(component.Type))
            {
                if (component.Position == null)
                {
                    return false;
                }
            }
            else
            {
                if (component.Position != null)
                {
                    return false;
                }
            }
            return true;
        }

        private bool IsDadanoComponent(ComponentTypes type)
        {
            return (type & (ComponentTypes.Meduvė | ComponentTypes.Pusmeduvė | ComponentTypes.Maitintuvė)) > 0;
        }

        private bool IsDaugiaaukštisComponent(ComponentTypes type)
        {
            return (type & (ComponentTypes.Aukštas | ComponentTypes.SkiriamojiTvorelė |
                            ComponentTypes.Išleistuvas | ComponentTypes.DugnoSklendė | ComponentTypes.Maitintuvė)) > 0;
        }

        private bool IsComponentMustHavePosition(ComponentTypes type)
        {
            return (type & (ComponentTypes.Aukštas | ComponentTypes.SkiriamojiTvorelė | ComponentTypes.Išleistuvas)) > 0;
        }

        private bool IsComponentSingle(ComponentTypes type)
        {
            return (type & (ComponentTypes.DugnoSklendė | ComponentTypes.Išleistuvas | ComponentTypes.SkiriamojiTvorelė)) > 0;
        }

        private bool IsComponentTypeValid(BeehiveComponent component)
        {
            if (component.Type == ComponentTypes.Meduvė || component.Type == ComponentTypes.Pusmeduvė ||
                component.Type == ComponentTypes.Aukštas || component.Type == ComponentTypes.SkiriamojiTvorelė ||
                component.Type == ComponentTypes.DugnoSklendė || component.Type == ComponentTypes.Išleistuvas ||
                component.Type == ComponentTypes.Maitintuvė)
            {
                return true;
            }
            return false;
        }
    }
}