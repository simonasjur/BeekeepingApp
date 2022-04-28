using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BeekeepingApi.Models
{
    public enum FrameType
    {
        NestFrame, HoneyFrame
    }

    public class NestExpansion
    {
        [Key]
        public long Id { get; set; }

        public DateTime Date { get; set; }

        public FrameType FrameType { get; set; }

        [Range(0, int.MaxValue)]
        public int CombSheets { get; set; }

        [Range(0, int.MaxValue)]
        public int Combs { get; set; }
        
        public long BeefamilyId { get; set; }

        [ForeignKey("BeefamilyId")]
        public BeeFamily Beefamily { get; set; }
    }
}
