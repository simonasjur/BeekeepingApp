using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BeekeepingApi.DTOs.BeehiveBeeFamilyDTOs
{
    public class BeehiveBeeFamilyCreateDTO
    {
        [Required]
        public DateTime ArriveDate { get; set; }

        public long BeehiveId { get; set; }

        public long BeeFamilyId { get; set; }
    }
}
