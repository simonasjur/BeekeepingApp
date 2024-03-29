﻿using System;
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
using BeekeepingApi.DTOs.UserDTOs;
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
                return BadRequest(new { message = "Naudotojas su slapyvardžiu \"" + userCreateDTO.Username + "\" jau egzistuoja." });
            }

            var existingEmail = await _context.Users.Where(u => u.Email.Equals(userCreateDTO.Email)).FirstOrDefaultAsync();
            if (existingEmail != null)
            {
                return BadRequest(new { message = "Naudotojas su el. paštu \"" + userCreateDTO.Email + "\" jau egzistuoja." });
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
                return BadRequest(new { message = "Neteisingas slapyvardis arba slaptažodis" });
            }
            if (!_userService.VerifyPassword(user, userAuthenticate.Password))
            {
                return BadRequest(new { message = "Neteisingas slapyvardis arba slaptažodis" });
            }

            UserWithTokenDTO userWithToken = _mapper.Map<UserWithTokenDTO>(user);
            _userService.Authenticate(userWithToken);

            return userWithToken;
        }

        [Authorize]
        [HttpPut("{id}/updatepassword")]
        public async Task<ActionResult> ChangeUserPassword(long id, ChangePasswordModel changePasswordModel)
        {
            var currentUserId = long.Parse(User.Identity.Name);
            if (id != currentUserId)
                return Forbid();

            if (id != changePasswordModel.Id)
            {
                return BadRequest();
            }

            var user = await _context.Users.FindAsync(id);

            if (!_userService.VerifyPassword(user, changePasswordModel.Password))
            {
                return BadRequest(new { message = "Neteisingas slaptažodis" });
            }

            if (!string.IsNullOrWhiteSpace(changePasswordModel.NewPassword))
            {
                _userService.CreateHashedPassword(user, changePasswordModel.NewPassword);
            }
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [Authorize]
        [HttpPut("{id}/updateemail")]
        public async Task<ActionResult> ChangeUserEmail(long id, ChangeUserEmailModel changeUserEmailModel)
        {
            var currentUserId = long.Parse(User.Identity.Name);
            if (id != currentUserId)
                return Forbid();

            if (id != changeUserEmailModel.Id)
            {
                return BadRequest();
            }

            var user = await _context.Users.FindAsync(id);

            if (!_userService.VerifyPassword(user, changeUserEmailModel.Password))
            {
                return BadRequest(new { message = "Neteisingas slaptažodis" });
            }
            _mapper.Map(changeUserEmailModel, user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        /*private bool UserExists(long id)
        {
            return _context.Users.Any(e => e.Id == id);
        }*/

    }
}
