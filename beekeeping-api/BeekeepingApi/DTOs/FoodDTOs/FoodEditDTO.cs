using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BeekeepingApi.DTOs.FoodDTOs
{
    public class FoodEditDTO
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public string? Name { get; set; }
    }
}
