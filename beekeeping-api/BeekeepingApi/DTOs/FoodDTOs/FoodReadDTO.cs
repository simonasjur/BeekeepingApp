using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeekeepingApi.DTOs.FoodDTOs
{
    public class FoodReadDTO
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public long FarmId { get; set; }
    }
}
