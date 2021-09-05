using AutoMapper;
using BeekeepingApi.DTOs.NestShorteningDTOs;
using BeekeepingApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeekeepingApi.Profiles
{
    public class NestShorteningProfile : Profile
    {
        public NestShorteningProfile()
        {
            CreateMap<NestShortening, NestShorteningReadDTO>();
            CreateMap<NestShorteningCreateDTO, NestShortening>();
            CreateMap<NestShorteningEditDTO, NestShortening>();
        }
    }
}
