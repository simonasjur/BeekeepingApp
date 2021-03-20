using AutoMapper;
using BeekeepingApi.DTOs.SuperDTOs;
using BeekeepingApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeekeepingApi.Profiles
{
    public class SuperProfile : Profile
    {
        public SuperProfile()
        {
            CreateMap<Super, SuperReadDTO>();
            CreateMap<SuperCreateDTO, Super>();
            CreateMap<SuperEditDTO, Super>();
        }
    }
}
