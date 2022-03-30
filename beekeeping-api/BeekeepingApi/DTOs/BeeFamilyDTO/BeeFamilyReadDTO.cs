using BeekeepingApi.DTOs.BeehiveDTOs;
using BeekeepingApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeekeepingApi.DTOs.BeeFamilyDTO
{
    public class BeeFamilyReadDTO
    {
        public long Id { get; set; }

        public bool IsNucleus { get; set; }

        public BeeFamilyOrigins Origin { get; set; }

        public BeeFamilyStates State { get; set; }

        public double? RequiredFoodForWinter { get; set; }

        public long FarmId { get; set; }

        //public BeehiveReadDTO Beehive { get; set; }
    }
}
