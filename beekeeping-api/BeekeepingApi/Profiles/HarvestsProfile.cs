using AutoMapper;
using BeekeepingApi.DTOs.HarvestDTOs;
using BeekeepingApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeekeepingApi.Profiles
{
    public class HarvestsProfile : Profile
    {
        public HarvestsProfile()
        {
            CreateMap<Harvest, HarvestReadDTO>();
            CreateMap<HarvestCreateDTO, Harvest>();
            CreateMap<HarvestEditDTO, Harvest>();
        }
    }
}
