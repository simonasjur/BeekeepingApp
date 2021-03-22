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
        Dadano, Daugiaaukštis
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
        public int No { get; set; }

        public DateTime AcquireDay { get; set; }

        public Colors Color { get; set; }

        public long FarmId { get; set; }

        [ForeignKey("FarmId")]
        public Farm Farm { get; set; }

        public virtual ICollection<Super> Supers { get; set; }

        public virtual ICollection<ApiaryBeehive> ApiaryBeehives { get; set; }
        public virtual ICollection<TodoItem> TodoItems { get; set; }
    }
}