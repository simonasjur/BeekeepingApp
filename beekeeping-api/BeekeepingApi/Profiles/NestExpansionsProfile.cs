using AutoMapper;
using BeekeepingApi.DTOs.NestExpansionDTOs;
using BeekeepingApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeekeepingApi.Profiles
{
    public class NestExpansionsProfile : Profile
    {
        public NestExpansionsProfile()
        {
            CreateMap<NestExpansion, NestExpansionReadDTO>();
            CreateMap<NestExpansionCreateDTO, NestExpansion>();
            CreateMap<NestExpansionEditDTO, NestExpansion>();
        }
    }
}
