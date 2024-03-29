﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BeekeepingApi.Models
{
    public class Feeding
    {
        [Key]
        public long Id { get; set; }

        public DateTime Date { get; set; }

        public double Quantity { get; set; }

        public long FoodId { get; set; }

        public long BeeFamilyId { get; set; }

        [ForeignKey("BeeFamilyId")]
        public BeeFamily BeeFamily { get; set; }

        [ForeignKey("FoodId")]
        public Food Food { get; set; }
    }
}
