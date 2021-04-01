using AutoMapper;
using BeekeepingApi.DTOs.ApiaryBeehiveDTOs;
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
            CreateMap<ApiaryBeehiveCreateDTO, ApiaryBeehive>();
            CreateMap<ApiaryBeehive, ApiaryBeehiveReadDTO>();
            CreateMap<ApiaryBeehiveEditDTO, ApiaryBeehive>();
            CreateMap<ApiaryBeehive, ApiaryBeehiveReadForApiaryDTO>();
            CreateMap<ApiaryBeehive, ApiaryBeehiveReadForBeehiveDTO>();
        }
    }
}
