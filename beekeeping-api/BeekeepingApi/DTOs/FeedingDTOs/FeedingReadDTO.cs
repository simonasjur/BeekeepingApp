using BeekeepingApi.DTOs.FoodDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeekeepingApi.DTOs.FeedingDTOs
{
    public class FeedingReadDTO
    {
        public long Id { get; set; }

        public DateTime Date { get; set; }

        public double Quantity { get; set; }

        public long FoodId { get; set; }

        public long BeeFamilyId { get; set; }

        public FoodReadDTO Food { get; set; }
    }
}
