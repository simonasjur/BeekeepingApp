using AutoMapper;
using BeekeepingApi.DTOs.NestShorteningDTOs;
using BeekeepingApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeekeepingApi.Profiles
{
    public class NestReductionsProfile : Profile
    {
        public NestReductionsProfile()
        {
            CreateMap<NestReduction, NestReductionReadDTO>();
            CreateMap<NestReductionCreateDTO, NestReduction>();
            CreateMap<NestReductionEditDTO, NestReduction>();
        }
    }
}
