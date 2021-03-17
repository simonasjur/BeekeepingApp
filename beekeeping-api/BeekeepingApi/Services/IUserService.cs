using BeekeepingApi.DTOs.UserDTOs;
using BeekeepingApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeekeepingApi.Services
{
    public interface IUserService
    {
        public void Authenticate(UserWithTokenDTO user);
        public void CreateHashedPassword(User user, string password);
        public bool VerifyPassword(User user, string password);
    }
}
