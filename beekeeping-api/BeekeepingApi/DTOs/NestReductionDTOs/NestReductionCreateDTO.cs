using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BeekeepingApi.DTOs.NestShorteningDTOs
{
    public class NestReductionCreateDTO
    {
        [Required]
        public DateTime? Date { get; set; }

        [Required, Range(0, int.MaxValue)]
        public int? StayedCombs { get; set; }

        [Required, Range(0, int.MaxValue)]
        public double? StayedHoney { get; set; }

        [Required, Range(0, int.MaxValue)]
        public int? StayedBroodCombs { get; set; }

        [Required, Range(0, int.MaxValue)]
        public double? RequiredFoodForWinter { get; set; }

        public long BeefamilyId { get; set; }
    }
}
