using AutoMapper;
using BeekeepingApi.DTOs.BeehiveBeeFamilyDTOs;
using BeekeepingApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeekeepingApi.Profiles
{
    public class BeehiveBeeFamilyProfile : Profile
    {
        public BeehiveBeeFamilyProfile()
        {
            CreateMap<BeehiveBeeFamilyCreateDTO, BeehiveBeeFamily>();
            CreateMap<BeehiveBeeFamily, BeehiveBeeFamilyReadDTO>();
            CreateMap<BeehiveBeeFamilyEditDTO, BeehiveBeeFamily>();
        }
    }
}
