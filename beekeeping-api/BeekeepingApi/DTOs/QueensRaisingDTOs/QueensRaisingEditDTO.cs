using BeekeepingApi.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BeekeepingApi.DTOs.QueensRaisingDTOs
{
    public class QueensRaisingEditDTO
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public DateTime? StartDate { get; set; }

        [Required, Range(0, int.MaxValue)]
        public int? LarvaCount { get; set; }

        [Required]
        public DevelopmentPlace? DevelopmentPlace { get; set; }

        [Required, Range(0, int.MaxValue)]
        public int? CappedCellCount { get; set; }

        [Required, Range(0, int.MaxValue)]
        public int? QueensCount { get; set; }
    }
}
