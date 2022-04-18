using AutoMapper;
using BeekeepingApi.DTOs.QueensRaisingDTOs;
using BeekeepingApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeekeepingApi.Profiles
{
    public class QueensRaisingsProfile : Profile
    {
        public QueensRaisingsProfile()
        {
            CreateMap<QueensRaising, QueensRaisingReadDTO>();
            CreateMap<QueensRaisingCreateDTO, QueensRaising>();
            CreateMap<QueensRaisingEditDTO, QueensRaising>();
        }
    }
}
