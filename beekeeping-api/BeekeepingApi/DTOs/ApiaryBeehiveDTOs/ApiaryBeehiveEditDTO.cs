using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BeekeepingApi.DTOs.ApiaryBeehiveDTOs
{
    public class ApiaryBeehiveEditDTO
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public DateTime ArriveDate { get; set; }

        [Required]
        public double X { get; set; }

        [Required]
        public double Y { get; set; }

        public DateTime? DepartDate { get; set; }
    }
}