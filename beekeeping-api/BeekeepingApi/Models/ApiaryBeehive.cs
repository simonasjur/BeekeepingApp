using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BeekeepingApi.Models
{
    public class ApiaryBeehive
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public DateTime ArriveDate { get; set; }

        [Required]
        public double X { get; set; }

        [Required]
        public double Y { get; set; }

        public DateTime DepartDate { get; set; }

        public long ApiaryId { get; set; }

        public long BeehiveId { get; set; }


        [ForeignKey("ApiaryId")]
        public Apiary Apiary { get; set; }

        [ForeignKey("BeehiveId")]
        public Beehive Beehive { get; set; }
    }
}