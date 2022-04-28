using AutoMapper;
using BeekeepingApi.DTOs.BeeFamilyQueenDTOs;
using BeekeepingApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeekeepingApi.Profiles
{
    public class BeeFamilyQueenProfile : Profile
    {
        public BeeFamilyQueenProfile()
        {
            CreateMap<BeefamilyQueen, BeeFamilyQueenReadDTO>();
            CreateMap<BeeFamilyQueenCreateDTO, BeefamilyQueen>();
            CreateMap<BeeFamilyQueenEditDTO, BeefamilyQueen>();
        }
    }
}
