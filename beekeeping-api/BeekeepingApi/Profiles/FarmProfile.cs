using AutoMapper;
using BeekeepingApi.DTOs.FarmDTOs;
using BeekeepingApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeekeepingApi.Profiles
{
    public class FarmProfile : Profile
    {
        public FarmProfile()
        {
            CreateMap<Farm, FarmReadDTO>();
            CreateMap<FarmCreateDTO, Farm>();
            CreateMap<FarmEditDTO, Farm>();
        }
    }
}
