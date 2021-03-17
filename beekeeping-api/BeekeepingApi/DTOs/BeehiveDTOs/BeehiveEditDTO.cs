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
        public BeehiveTypes Type { get; set; }

        [Required]
        public int No { get; set; }

        public DateTime AcquireDay { get; set; }

        public Colors Color { get; set; }

        public long FarmId { get; set; }
    }
}
