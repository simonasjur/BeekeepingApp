using BeekeepingApi.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BeekeepingApi.DTOs.BeehiveDTOs
{
    public class BeehiveEditDTO
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public bool? IsEmpty { get; set; }

        [Range(0, int.MaxValue)]
        public int? No { get; set; }

        public DateTime? AcquireDay { get; set; }

        public Colors? Color { get; set; }

        [Range(0, 30)]
        public int? MaxNestCombs { get; set; }

        [Range(0, 30)]
        public int? NestCombs { get; set; }

        [Range(0, 4)]
        public int? MaxHoneyCombsSupers { get; set; }
    }
}
