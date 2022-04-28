using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BeekeepingApi.DTOs.BeeFamilyQueenDTOs
{
    public class BeeFamilyQueenCreateDTO
    {
        [Required]
        public DateTime? InsertDate { get; set; }

        public long QueenId { get; set; }

        public long BeeFamilyId { get; set; }
    }
}
