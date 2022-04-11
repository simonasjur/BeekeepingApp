using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BeekeepingApi.Models
{
    public enum BeeProduct
    {
        BeeBread, Beeswax, Propolis, Honey
    }

    public class Harvest
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public BeeProduct Product { get; set; }

        [Column(TypeName = "date")]
        [DisplayFormat(DataFormatString = "{yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }

        [Column(TypeName = "date")]
        [DisplayFormat(DataFormatString = "{yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime EndDate { get; set; }
        
        [Required]
        public double Quantity { get; set; }

        public long FarmId { get; set; }

        [ForeignKey("FarmId")]
        public Farm Farm { get; set; }

        public long? ApiaryId { get; set; }

        [ForeignKey("ApiaryId")]
        public Apiary Apiary { get; set; }

        public long? BeeFamilyId { get; set; }

        [ForeignKey("BeeFamilyId")]
        public BeeFamily BeeFamily { get; set; }
    }
}
