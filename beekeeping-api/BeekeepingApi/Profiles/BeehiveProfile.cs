using AutoMapper;
using BeekeepingApi.DTOs.BeehiveDTOs;
using BeekeepingApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeekeepingApi.Profiles
{
    public class BeehiveProfile : Profile
    {
        public BeehiveProfile()
        {
            CreateMap<Beehive, BeehiveReadDTO>();
            CreateMap<BeehiveCreateDTO, Beehive>();
            CreateMap<BeehiveEditDTO, Beehive>();
        }
    }
}
