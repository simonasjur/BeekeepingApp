using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BeekeepingApi.DTOs.ApiaryBeehiveDTOs
{
    public class ApiaryBeehiveCreateDTO
    {
        [Required]
        public DateTime ArriveDate { get; set; }

        [Required]
        public double X { get; set; }

        [Required]
        public double Y { get; set; }

        public long ApiaryId { get; set; }

        public long BeehiveId { get; set; }
    }
}
