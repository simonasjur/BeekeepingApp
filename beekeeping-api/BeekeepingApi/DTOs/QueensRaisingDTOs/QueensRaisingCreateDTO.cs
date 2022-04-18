using BeekeepingApi.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BeekeepingApi.DTOs.QueensRaisingDTOs
{
    public class QueensRaisingCreateDTO
    {
        [Required]
        public DateTime? StartDate { get; set; }

        [Required, Range(0, int.MaxValue)]
        public int? LarvaCount { get; set; }

        [Required]
        public DevelopmentPlace? DevelopmentPlace { get; set; }

        public long MotherId { get; set; }

        public long BeefamilyId { get; set; }
    }
}
