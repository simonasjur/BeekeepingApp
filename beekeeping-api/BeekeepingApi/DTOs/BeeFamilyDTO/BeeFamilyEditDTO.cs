using BeekeepingApi.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BeekeepingApi.DTOs.BeeFamilyDTO
{
    public class BeeFamilyEditDTO
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public BeeFamilyOrigins? Origin { get; set; }

        [Required]
        public BeeFamilyStates? State { get; set; }

        public double? RequiredFoodForWinter { get; set; }
    }
}
