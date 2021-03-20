using AutoMapper;
using BeekeepingApi.DTOs.ApiaryDTOs;
using BeekeepingApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeekeepingApi.Profiles
{
    public class ApiaryProfile : Profile
    {
        public ApiaryProfile()
        {
            CreateMap<Apiary, ApiaryReadDTO>();
            CreateMap<ApiaryCreateDTO, Apiary>();
            CreateMap<ApiaryEditDTO, Apiary>();
        }
    }
}
