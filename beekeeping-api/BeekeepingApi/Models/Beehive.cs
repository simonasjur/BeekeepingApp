using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BeekeepingApi.Models
{
    public enum BeehiveTypes
    {
        Dadano, Daugiaaukštis, NukleosoSekcija
    }

    public enum Colors
    {
        Mėlyna, Geltona, Balta, Raudona, Žalia
    }
    public class Beehive
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public BeehiveTypes Type { get; set; }

        [Required]
        public bool IsEmpty { get; set; }

        [Range(0, int.MaxValue)]
        public int? No { get; set; }

        public DateTime? AcquireDay { get; set; }

        public Colors? Color { get; set; }

        [Range(0, 30)]
        public int? MaxNestCombs { get; set; }

        [Range(0, 30)]
        public int? NestCombs { get; set; }

        [Range(0, 4)]
        public int? MaxHoneyCombsSupers { get; set; }

        public long FarmId { get; set; }

        [ForeignKey("FarmId")]
        public Farm Farm { get; set; }

        public virtual ICollection<BeehiveBeeFamily> BeehiveBeeFamilies { get; set; }
        public virtual ICollection<BeehiveComponent> Components { get; set; }
    }
}
