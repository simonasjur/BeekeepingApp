using AutoMapper;
using BeekeepingApi.DTOs.QueenDTOs;
using BeekeepingApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeekeepingApi.Profiles
{
    public class QueenProfile : Profile
    {
        public QueenProfile()
        {
            CreateMap<Queen, QueenReadDTO>();
            CreateMap<QueenCreateDTO, Queen>();
            CreateMap<QueenEditDTO, Queen>();
        }
    }
}
