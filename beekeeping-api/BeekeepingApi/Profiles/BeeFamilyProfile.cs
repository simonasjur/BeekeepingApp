using AutoMapper;
using BeekeepingApi.DTOs.BeeFamilyDTO;
using BeekeepingApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeekeepingApi.Profiles
{
    public class BeeFamilyProfile : Profile
    {
        public BeeFamilyProfile()
        {
            CreateMap<BeeFamily, BeeFamilyReadDTO>();
            CreateMap<BeeFamilyCreateDTO, BeeFamily>();
            CreateMap<BeeFamilyEditDTO, BeeFamily>();
        }
    }
}
