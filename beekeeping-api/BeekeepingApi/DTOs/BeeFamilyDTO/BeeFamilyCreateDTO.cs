using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using BeekeepingApi.Models;

namespace BeekeepingApi.DTOs.BeeFamilyDTO
{
    public class BeeFamilyCreateDTO
    {
        [Required]
        public BeeFamilyOrigins? Origin { get; set; }

        public long FarmId { get; set; }
    }
}
