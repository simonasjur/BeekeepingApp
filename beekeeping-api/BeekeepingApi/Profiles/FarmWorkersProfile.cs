using AutoMapper;
using BeekeepingApi.DTOs.FarmWorkerDTOs;
using BeekeepingApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeekeepingApi.Profiles
{
    public class FarmWorkersProfile : Profile
    {
        public FarmWorkersProfile()
        {
            CreateMap<FarmWorker, FarmWorkerReadDTO>();
            CreateMap<FarmWorkerCreateDTO, FarmWorker>();
        }
    }
}
