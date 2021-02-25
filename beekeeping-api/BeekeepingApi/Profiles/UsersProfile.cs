using AutoMapper;
using BeekeepingApi.DTOs.UsersDTOs;
using BeekeepingApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeekeepingApi.Profiles
{
    public class UsersProfile : Profile
    {
        public UsersProfile()
        {
            CreateMap<User, UserReadDTO>();
            CreateMap<UserCreateDTO, User>();
            CreateMap<User, UserWithTokenDTO>();
            CreateMap<UserEditDTO, User>();
        }
    }
}
