using BeekeepingApi.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BeekeepingApi.DTOs.BeehiveComponentDTOs
{
    public class BeehiveComponentEditDTO
    {
        [Key]
        public long Id { get; set; }
        public ComponentTypes Type { get; set; }

        [Range(1, 9)]
        public int? Position { get; set; }

        public DateTime InstallationDate { get; set; }
    }
}
