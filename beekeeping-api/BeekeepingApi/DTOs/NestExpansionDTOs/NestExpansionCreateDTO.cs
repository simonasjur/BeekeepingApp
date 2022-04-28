using BeekeepingApi.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BeekeepingApi.DTOs.NestExpansionDTOs
{
    public class NestExpansionCreateDTO
    {
        [Required]
        public DateTime? Date { get; set; }

        [Required]
        public FrameType? FrameType { get; set; }

        [Required, Range(0, int.MaxValue)]
        public int? CombSheets { get; set; }

        [Required, Range(0, int.MaxValue)]
        public int? Combs { get; set; }

        public long BeefamilyId { get; set; }
    }
}
