using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BeekeepingApi.Models
{
    public enum BeeFamilyStates
    {
        Gyvena, Išsispietus, Išmirusi, SujungtaSuKitaŠeima
    }

    public enum BeeFamilyOrigins
    {
        Spiečius, IšKitosŠeimos, Pirkta, Padovanota
    }

    public class BeeFamily
    {
        [Key]
        public long Id { get; set; }

        public bool IsNucleus { get; set; }

        [Required]
        public BeeFamilyOrigins Origin { get; set; }

        [Required]
        public BeeFamilyStates State { get; set; }

        public double? RequiredFoodForWinter { get; set; }

        public long FarmId { get; set; }

        [ForeignKey("FarmId")]
        public Farm Farm { get; set; }

        public virtual ICollection<ApiaryBeeFamily> ApiaryBeeFamilies { get; set; }
        public virtual ICollection<BeehiveBeeFamily> BeehiveBeeFamilies { get; set; }
        public virtual ICollection<TodoItem> TodoItems { get; set; }
        public virtual ICollection<NestShortening> NestShortenings { get; set; }
        public virtual ICollection<Feeding> Feedings { get; set; }
    }
}