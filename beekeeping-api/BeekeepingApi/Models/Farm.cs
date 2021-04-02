using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BeekeepingApi.Models
{
    public class Farm
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Column(TypeName = "date")]
        [DisplayFormat(DataFormatString = "{yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime CreationDate { get; set; }

        public virtual ICollection<FarmWorker> FarmWorkers { get; set; }
        public virtual ICollection<Apiary> Apiaries { get; set; }
        public virtual ICollection<Beehive> Beehives { get; set; }
        public virtual ICollection<Harvest> Harvests { get; set; }
        public virtual ICollection<TodoItem> TodoItems { get; set; }
        public virtual ICollection<Food> Foods { get; set; }
    }
}
