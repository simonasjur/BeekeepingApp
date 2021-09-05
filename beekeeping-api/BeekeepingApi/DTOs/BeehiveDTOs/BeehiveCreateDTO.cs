using BeekeepingApi.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BeekeepingApi.DTOs.BeehiveDTOs
{
    public class BeehiveCreateDTO
    {
        [Required]
        public BeehiveTypes? Type { get; set; }

        [Required]
        public int? No { get; set; }

        [Required]
        public bool? IsEmpty { get; set; }

        public DateTime? AcquireDay { get; set; }

        public Colors? Color { get; set; }

        public int? NestCombs { get; set; }

        [Required]
        public long? FarmId { get; set; }
    }
}
