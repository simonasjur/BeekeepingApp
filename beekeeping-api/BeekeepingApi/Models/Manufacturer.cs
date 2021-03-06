using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BeekeepingApi.Models
{
    public class Manufacturer
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public string Name { get; set; }

        public virtual ICollection<Beehive> Beehives { get; set; }
    }
}