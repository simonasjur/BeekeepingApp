using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BeekeepingApi.Models
{
    public class NestReduction
    {
        [Key]
        public long Id { get; set; }

        public DateTime Date { get; set; }

        [Range(0, int.MaxValue)]
        public int StayedCombs{ get; set; }

        [Range(0, int.MaxValue)]
        public double StayedHoney { get; set; }

        [Range(0, int.MaxValue)]
        public int StayedBroodCombs { get; set; }

        [Range(0, int.MaxValue)]
        public double RequiredFoodForWinter { get; set; }

        public long BeefamilyId { get; set; }

        [ForeignKey("BeefamilyId")]
        public BeeFamily BeeFamily { get; set; }
    }
}
