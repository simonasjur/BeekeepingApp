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

        public long BeehiveId { get; set; }

        public long ApiaryId { get; set; }

        public DateTime ArriveDate { get; set; }

        [Range(1, 20)]
        public int NestCombs { get; set; }

        [Range(1, 9)]
        public int SupersCount { get; set; }
    }
}
