using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BeekeepingApi.DTOs.ApiaryBeeFamilyDTOs
{
    public class ApiaryBeeFamilyCreateDTO
    {
        [Required]
        public DateTime ArriveDate { get; set; }

        public long ApiaryId { get; set; }

        public long BeeFamilyId { get; set; }
    }
}
