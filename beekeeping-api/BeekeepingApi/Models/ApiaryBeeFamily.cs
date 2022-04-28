using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BeekeepingApi.Models
{
    public class ApiaryBeeFamily
    {
        [Key]
        public long Id { get; set; }

        public DateTime ArriveDate { get; set; }

        public DateTime? DepartDate { get; set; }

        public long ApiaryId { get; set; }

        public long BeeFamilyId { get; set; }


        [ForeignKey("ApiaryId")]
        public Apiary Apiary { get; set; }

        [ForeignKey("BeeFamilyId")]
        public BeeFamily BeeFamily { get; set; }
    }
}