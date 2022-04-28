using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BeekeepingApi.DTOs.BeeFamilyQueenDTOs
{
    public class BeeFamilyQueenEditDTO
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public DateTime? InsertDate { get; set; }

        public DateTime? TakeOutDate { get; set; }
    }
}
