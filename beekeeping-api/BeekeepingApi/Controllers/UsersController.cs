using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BeekeepingApi.Models;
using AutoMapper;
using BeekeepingApi.Services;
using Microsoft.AspNetCore.Authorization;
using BeekeepingApi.Helpers;
using BeekeepingApi.DTOs.UsersDTOs;
using BeekeepingApi.DTOs.FarmWorkerDTOs;
using BeekeepingApi.DTOs.FarmDTOs;

//SJ
namespace BeekeepingApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly BeekeepingContext _context;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;

        public UsersController(BeekeepingContext context, IMapper mapper, IUserService userService)
        {
            _context = context;
            _mapper = mapper;
            _userService = userService;
        }

        // GET: api/Users/1/FarmWorkers
        [Authorize]
        [HttpGet("{id}/FarmWorkers")]
        public async Task<ActionResult<IEnumerable<FarmWorkerReadDTO>>> GetUserFarmWorkers(long id)
        {
            var currentUserId = long.Parse(User.Identity.Name);
            if (id != currentUserId)
                return Forbid();

            var user = await _context.Users.FindAsync(id);

            var farmWorkersList = await _context.FarmWorkers.Where(l => l.UserId == id).ToListAsync();

            return _mapper.Map<IEnumerable<FarmWorkerReadDTO>>(farmWorkersList).ToList();
        }

        // DELETE: api/Users/5/FarmWorkers/1
        [Authorize]
        [HttpDelete("{userId}/FarmWorkers/{workerId}")]
        public async Task<ActionResult<FarmWorkerReadDTO>> DeleteUserFarmWorker(long userId, long workerId)
        {
            var currentUserId = long.Parse(User.Identity.Name);
            if (userId != currentUserId)
                return Forbid();

            var farmWorker = await _context.FarmWorkers.FindAsync(userId, workerId);
            if (farmWorker == null)
                return NotFound();

            if (farmWorker.UserId != currentUserId)
                return Forbid();

            _context.FarmWorkers.Remove(farmWorker);
            await _context.SaveChangesAsync();

            return _mapper.Map<FarmWorkerReadDTO>(farmWorker);
        }

        // GET: api/Users/1/Farms
        [Authorize]
        [HttpGet("{id}/Farms")]
        public async Task<ActionResult<IEnumerable<FarmReadDTO>>> GetUserFarms(long id)
        {
            var currentUserId = long.Parse(User.Identity.Name);
            if (id != currentUserId)
                return Forbid();

            var user = await _context.Users.FindAsync(id);

            var farmWorkersList = await _context.FarmWorkers.Where(l => l.UserId == id).ToListAsync();
            var farmList = new List<Farm>();
            foreach (var farmWorker in farmWorkersList)
            {
                var farm = await _context.Farms.FindAsync(farmWorker.FarmId);
                farmList.Add(farm);
            }

            return _mapper.Map<IEnumerable<FarmReadDTO>>(farmList).ToList();
        }

        // GET: api/Users/1/Farms/1
        [Authorize]
        [HttpGet("{userId}/Farms/{farmId}")]
        public async Task<ActionResult<FarmReadDTO>> GetUserFarm(long userId, long farmId)
        {
            var currentUserId = long.Parse(User.Identity.Name);
            if (userId != currentUserId)
                return Forbid();

            var farm = await _context.Farms.FindAsync(farmId);
            var farmWorker = await _context.FarmWorkers.FindAsync(userId, farmId);

            if (farm == null)
                return NotFound();

            if (farmWorker == null)
                return Forbid();

            return _mapper.Map<FarmReadDTO>(farm);
        }

        // POST: api/Users/5/Farms
        [Authorize]
        [HttpPost("{id}/Farms")]
        public async Task<ActionResult<FarmReadDTO>> PostUserFarm(long id, FarmCreateDTO farmCreateDTO)
        {
            var user = await _context.Users.FindAsync(id);
            var currentUserId = long.Parse(User.Identity.Name);

            if (user == null || user.Id != currentUserId)
                return Forbid();

            var farm = _mapper.Map<Farm>(farmCreateDTO);
            _context.Farms.Add(farm);
            await _context.SaveChangesAsync();

            var farmWorker = new FarmWorker
            {
                Role = WorkerRole.Owner,
                FarmId = farm.Id,
                UserId = user.Id
            };
            _context.FarmWorkers.Add(farmWorker);
            await _context.SaveChangesAsync();

            var farmReadDTO = _mapper.Map<FarmReadDTO>(farm);

            return CreatedAtAction("GetUserFarm", "Users", new { userId = user.Id, farmId = farm.Id }, farmReadDTO);
        }

        // PUT: api/Users/5/Farms/1
        [Authorize]
        [HttpPut("{userId}/Farms/{farmId}")]
        public async Task<IActionResult> PutUserFarm(long userId, long farmId, FarmEditDTO farmEditDTO)
        {
            var currentUserId = long.Parse(User.Identity.Name);
            if (userId != currentUserId)
                return Forbid();

            if (farmId != farmEditDTO.Id)
            {
                return BadRequest();
            }

            var farm = await _context.Farms.FindAsync(farmId);
            var farmWorker = await _context.FarmWorkers.FindAsync(userId, farmId);

            if (farm == null)
                return NotFound();

            if (farmWorker == null)
                return Forbid();

            _mapper.Map(farmEditDTO, farm);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Users/5/Farms/5
        [Authorize]
        [HttpDelete("{userId}/Farms/{farmId}")]
        public async Task<ActionResult<FarmReadDTO>> DeleteUserFarm(long userId, long farmId)
        {
            var currentUserId = long.Parse(User.Identity.Name);
            if (userId != currentUserId)
                return Forbid();

            var farm = await _context.Farms.FindAsync(farmId);
            var farmWorker = await _context.FarmWorkers.FindAsync(userId, farmId);

            if (farm == null)
                return NotFound();

            if (farmWorker == null)
                return Forbid();

            _context.Farms.Remove(farm);
            await _context.SaveChangesAsync();

            return _mapper.Map<FarmReadDTO>(farm);
        }

        // GET: api/Users
        /*[Authorize(Roles = Role.Admin)]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserReadDTO>>> GetUsers()
        {
            var userList = await _context.Users.ToListAsync();

            return _mapper.Map<IEnumerable<UserReadDTO>>(userList).ToList();
        }*/

        // GET: api/Users/5

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<UserReadDTO>> GetUser(long id)
        {
            var currentUserId = long.Parse(User.Identity.Name);
            if (id != currentUserId && !User.IsInRole(Role.Admin))
                return Forbid();

            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return _mapper.Map<UserReadDTO>(user);
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(long id, UserEditDTO userEditDTO)
        {
            var currentUserId = long.Parse(User.Identity.Name);
            if (id != currentUserId)
                return Forbid();

            if (id != userEditDTO.Id)
            {
                return BadRequest();
            }

            var user = await _context.Users.FindAsync(id);

            _mapper.Map(userEditDTO, user);
            if (!string.IsNullOrWhiteSpace(userEditDTO.Password))
            {
                _userService.CreateHashedPassword(user, userEditDTO.Password);
            }
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // POST: api/Users
        [HttpPost]
        public async Task<ActionResult<UserReadDTO>> CreateUser(UserCreateDTO userCreateDTO)
        {
            var existingUser = await _context.Users.Where(u => u.Username.Equals(userCreateDTO.Username)).FirstOrDefaultAsync();
            if (existingUser != null)
            {
                return BadRequest(new { message = "User with username \"" + userCreateDTO.Username + "\" already exist." });
            }

            var existingEmail = await _context.Users.Where(u => u.Email.Equals(userCreateDTO.Email)).FirstOrDefaultAsync();
            if (existingEmail != null)
            {
                return BadRequest(new { message = "User with email \"" + userCreateDTO.Email + "\" already exist." });
            }

            var user = _mapper.Map<User>(userCreateDTO);

            user.Role = Role.User;
            _userService.CreateHashedPassword(user, userCreateDTO.Password);
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var returningUser = _mapper.Map<UserReadDTO>(user);

            return CreatedAtAction("GetUser", new { id = user.Id }, returningUser);
        }

        // DELETE: api/Users/5
        /*[Authorize(Roles = Role.Admin)]
        [HttpDelete("{id}")]
        public async Task<ActionResult<UserReadDTO>> DeleteUser(long id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            bool isUserCanBeDeleted = true;
            String errorMessage = "User can't be deleted. Reasons:";
            if (user.Role == Role.Admin)
            {
                isUserCanBeDeleted = false;
                errorMessage += "\n - User is admin.";
            }

            if (!isUserCanBeDeleted)
            {
                return BadRequest(new { message = errorMessage });
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return _mapper.Map<UserReadDTO>(user);
        }*/

        [HttpPost("authenticate")]
        public async Task<ActionResult<UserWithTokenDTO>> AuthenticateUser(UserAuthenticate userAuthenticate)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Username.Equals(userAuthenticate.Username));
            if (user == null)
            {
                return BadRequest(new { message = "Username is incorrect" });
            }
            if (!_userService.VerifyPassword(user, userAuthenticate.Password))
            {
                return BadRequest(new { message = "Password is incorrect" });
            }

            UserWithTokenDTO userWithToken = _mapper.Map<UserWithTokenDTO>(user);
            _userService.Authenticate(userWithToken);

            return userWithToken;
        }

        /*private bool UserExists(long id)
        {
            return _context.Users.Any(e => e.Id == id);
        }*/

    }
}
