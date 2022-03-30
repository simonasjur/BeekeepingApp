using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BeekeepingApi.DTOs.ApiaryBeeFamilyDTOs
{
    public class ApiaryBeeFamilyEditDTO
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public DateTime ArriveDate { get; set; }

        public DateTime? DepartDate { get; set; }
    }
}