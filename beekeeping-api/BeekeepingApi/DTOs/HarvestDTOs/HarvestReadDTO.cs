using BeekeepingApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeekeepingApi.DTOs.HarvestDTOs
{
    public class HarvestReadDTO
    {
        public long Id { get; set; }
        public BeeProduct Product { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public double Quantity { get; set; }
        public long FarmId { get; set; }
        public long ApiaryId { get; set; }
        public long BeeFamilyId { get; set; }
    }
}
