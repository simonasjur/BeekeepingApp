using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BeekeepingApi.Models
{
    [Flags]
    public enum ComponentTypes
    {
        Meduvė = 1,
        Pusmeduvė = 2,
        Išleistuvas = 4,
        SkiriamojiTvorelė = 8,
        Aukštas = 16,
        DugnoSklendė = 32,
        Maitintuvė = 64
    }

    public class BeehiveComponent
    {
        [Key]
        public long Id { get; set; }

        public ComponentTypes Type { get; set; }

        [Range(1, 9)]
        public int? Position { get; set; }

        public DateTime InstallationDate { get; set; }

        public long BeehiveId { get; set; }

        [ForeignKey("BeehiveId")]
        public Beehive Beehive { get; set; }
    }
}