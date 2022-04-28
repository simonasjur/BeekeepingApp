using AutoMapper;
using BeekeepingApi.DTOs.FoodDTOs;
using BeekeepingApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeekeepingApi.Profiles
{
    public class FoodProfile : Profile
    {
        public FoodProfile()
        {
            CreateMap<Food, FoodReadDTO>();
            CreateMap<FoodCreateDTO, Food>();
            CreateMap<FoodEditDTO, Food>();
        }
    }
}
