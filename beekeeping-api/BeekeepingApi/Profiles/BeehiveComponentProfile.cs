using AutoMapper;
using BeekeepingApi.DTOs.BeehiveComponentDTOs;
using BeekeepingApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeekeepingApi.Profiles
{
    public class BeehiveComponentProfile : Profile
    {
        public BeehiveComponentProfile()
        {
            CreateMap<BeehiveComponent, BeehiveComponentReadDTO>();
            CreateMap<BeehiveComponentCreateDTO, BeehiveComponent>();
            CreateMap<BeehiveComponentEditDTO, BeehiveComponent>();
        }
    }
}
