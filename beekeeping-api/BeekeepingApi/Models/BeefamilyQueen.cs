using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BeekeepingApi.Models
{
    public class BeefamilyQueen
    {
        [Key]
        public long Id { get; set; }

        public DateTime InsertDate { get; set; }

        public DateTime? TakeOutDate { get; set; }

        public long QueenId { get; set; }

        public long BeeFamilyId { get; set; }

        [ForeignKey("QueenId")]
        public Queen Queen { get; set; }

        [ForeignKey("BeeFamilyId")]
        public BeeFamily BeeFamily { get; set; }
    }
}
