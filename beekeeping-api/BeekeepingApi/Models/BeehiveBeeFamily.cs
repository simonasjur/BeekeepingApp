using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BeekeepingApi.Models
{
    public class BeehiveBeeFamily
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public DateTime ArriveDate { get; set; }

        public DateTime? DepartDate { get; set; }

        public long BeehiveId { get; set; }

        public long BeeFamilyId { get; set; }


        [ForeignKey("BeehiveId")]
        public Beehive Beehive { get; set; }

        [ForeignKey("BeeFamilyId")]
        public BeeFamily BeeFamily { get; set; }
    }
}
