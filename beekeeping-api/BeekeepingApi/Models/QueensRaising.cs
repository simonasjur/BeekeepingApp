using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BeekeepingApi.Models
{
    public enum DevelopmentPlace
    {
        Beehive, Incubator
    }
    public class QueensRaising
    {
        [Key]
        public long Id { get; set; }

        public DateTime StartDate { get; set; }

        [Range(0, int.MaxValue)]
        public int LarvaCount { get; set; }

        public DevelopmentPlace DevelopmentPlace { get; set; }

        [Range(0, int.MaxValue)]
        public int CappedCellCount { get; set; }

        [Range(0, int.MaxValue)]
        public int QueensCount { get; set; }

        public long MotherId { get; set; }

        public long BeefamilyId { get; set; }

        [ForeignKey("MotherId")]
        public Queen Mother { get; set; }

        [ForeignKey("BeefamilyId")]
        public BeeFamily Beefamily { get; set; }
    }
}
