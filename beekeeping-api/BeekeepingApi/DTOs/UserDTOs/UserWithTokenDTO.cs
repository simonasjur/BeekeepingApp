using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeekeepingApi.DTOs.UserDTOs
{
    public class UserWithTokenDTO : UserReadDTO
    {
        public string Token { get; set; }
    }
}
