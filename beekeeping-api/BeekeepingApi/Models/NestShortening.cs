using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BeekeepingApi.Models
{
    public class NestShortening
    {
        [Key]
        public long Id { get; set; }

        public DateTime Date { get; set; }

        public int StayedCombs{ get; set; }

        public double StayedHoney { get; set; }

        public int StayedBroodCombs { get; set; }

        public long BeehiveId { get; set; }

        [ForeignKey("BeehiveId")]
        public Beehive Beehive { get; set; }
    }
}
