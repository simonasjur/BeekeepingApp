﻿using BeekeepingApi.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BeekeepingApi.DTOs.HarvestDTOs
{
    public class HarvestCreateDTO
    {
        [Required]
        public BeeProduct Product { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        [Required]
        public double Quantity { get; set; }

        public long FarmId { get; set; }
        public long? ApiaryId { get; set; }
        public long? BeeFamilyId { get; set; }
    }
}
