using AutoMapper;
using BeekeepingApi.DTOs.InvitationDTOs;
using BeekeepingApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BeekeepingApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class InvitationsController : ControllerBase
    {
        private readonly BeekeepingContext _context;
        private readonly IMapper _mapper;

        public InvitationsController(BeekeepingContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Farms/1/Invitation
        [HttpGet("/api/Farms/{farmId}/Invitation")]
        public async Task<ActionResult<InvitationReadDTO>> GetFarmInvitation(long farmId)
        {
            var farm = await _context.Farms.FindAsync(farmId);
            if (farm == null)
                return NotFound();

            var currentUserId = long.Parse(User.Identity.Name);
            var farmWorker = await _context.FarmWorkers.FindAsync(currentUserId, farmId);
            if (farmWorker == null)
                return Forbid();

            var invitation = await _context.Invitations.FindAsync(farmId);

            if (invitation == null)
            {
                var invitationToSave = new Invitation
                {
                    ExpirationDate = DateTime.UtcNow.AddMinutes(10),
                    Code = Guid.NewGuid(),
                    FarmId = farmId
                };
                await _context.Invitations.AddAsync(invitationToSave);
                _context.SaveChanges();

                invitation = await _context.Invitations.FindAsync(farmId);
            }

            if (invitation.ExpirationDate < DateTime.UtcNow)
            {
                //edit old one
                invitation.Code = Guid.NewGuid();
                invitation.ExpirationDate = DateTime.UtcNow.AddMinutes(10);
                _context.SaveChanges();
            }

            return _mapper.Map<InvitationReadDTO>(invitation);
        }


        [HttpGet("/api/Invitation/{code}")]
        public async Task<IActionResult> ValidateInvitation(string code)
        {
            var invitation = await _context.Invitations.Where(l => l.Code.ToString() == code).FirstOrDefaultAsync();
            if (invitation == null)
                return NotFound("Neteisingas kodas");

            if (invitation.ExpirationDate < DateTime.UtcNow)
                return Forbid("Pakvietimo galiojimo laikas pasibaigė");

            var currentUserId = long.Parse(User.Identity.Name);

            var farmWorker = await _context.FarmWorkers.FindAsync(currentUserId, invitation.FarmId);
            if (farmWorker != null)
                return Forbid("Jūs jau esate užregistruotas šiame ūkyje");

            farmWorker = new FarmWorker
            {
                Role = WorkerRole.Assistant,
                FarmId = invitation.FarmId,
                UserId = currentUserId
            };
            _context.FarmWorkers.Add(farmWorker);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
