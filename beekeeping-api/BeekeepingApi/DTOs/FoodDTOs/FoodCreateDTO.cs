using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BeekeepingApi.DTOs.FoodDTOs
{
    public class FoodCreateDTO
    {
        [Required]
        public string? Name { get; set; }

        public long FarmId { get; set; }
    }
}
