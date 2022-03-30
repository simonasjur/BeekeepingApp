using AutoMapper;
using BeekeepingApi.DTOs.ApiaryBeeFamilyDTOs;
using BeekeepingApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeekeepingApi.Profiles
{
    public class ApiaryBeehiveProfile : Profile
    {
        public ApiaryBeehiveProfile()
        {
            CreateMap<ApiaryBeeFamilyCreateDTO, ApiaryBeeFamily>();
            CreateMap<ApiaryBeeFamily, ApiaryBeeFamilyReadDTO>();
            CreateMap<ApiaryBeeFamilyEditDTO, ApiaryBeeFamily>();
            CreateMap<ApiaryBeeFamily, ApiaryBeeFamilyReadForApiaryDTO>();
            CreateMap<ApiaryBeeFamily, ApiaryBeeFamilyReadForBeeFamilyDTO>();
        }
    }
}
