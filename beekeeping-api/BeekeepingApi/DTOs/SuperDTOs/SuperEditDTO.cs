using BeekeepingApi.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BeekeepingApi.DTOs.SuperDTOs
{
    public class SuperEditDTO
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public int Position { get; set; }

        [Required]
        public Colors Color { get; set; }

        [Required]
        public DateTime InstallationDate { get; set; }

        public long BeehiveId { get; set; }
    }
}
