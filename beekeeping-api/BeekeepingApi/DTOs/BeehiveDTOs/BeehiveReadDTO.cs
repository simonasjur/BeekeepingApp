using BeekeepingApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeekeepingApi.DTOs.BeehiveDTOs
{
    public class BeehiveReadDTO
    {
        public long Id { get; set; }

        public BeehiveTypes Type { get; set; }

        public int No { get; set; }

        public bool IsEmpty { get; set; }

        public DateTime? AcquireDay { get; set; }

        public Colors? Color { get; set; }

        public int? NestCombs { get; set; }

        public double? RequiredFoodForWinter { get; set; }

        public long FarmId { get; set; }
    }
}
