using BeekeepingApi.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BeekeepingApi.DTOs.HarvestDTOs
{
    public class HarvestEditDTO
    {
        [Key]
        public long Id { get; set; }

        public BeeProduct Product { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public double Quantity { get; set; }
    }
}
