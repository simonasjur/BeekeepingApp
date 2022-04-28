using AutoMapper;
using BeekeepingApi.DTOs.FeedingDTOs;
using BeekeepingApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeekeepingApi.Profiles
{
    public class FeedingProfile : Profile
    {
        public FeedingProfile()
        {
            CreateMap<Feeding, FeedingReadDTO>();
            CreateMap<FeedingCreateDTO, Feeding>();
            CreateMap<FeedingEditDTO, Feeding>();
        }
    }
}
