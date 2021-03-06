using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BeekeepingApi.Models
{
    public class Apiary
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public double Longitude { get; set; }

        [Required]
        public double Latitude { get; set; }

        public long FarmId { get; set; }


        [ForeignKey("FarmId")]
        public Farm Farm { get; set; }

        public virtual ICollection<ApiaryBeehive> ApiaryBeehives { get; set; }
    }
}