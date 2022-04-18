using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BeekeepingApi.Models
{
    public enum Breed
    {
        Bakfasto, Karnika, Mišrūnė, Nežinoma
    }

    public enum QueenState
    {
        Lopšys = 1, //Cell
        GyvenaAvilyje = 2,
        PriduodamaŠeimai = 4,
        IzoliuotaNarvelyje = 8,
        Parduota = 16,
        Išsispietusi = 32,
        Numirusi = 64
    }

    public class Queen
    {
        [Key]
        public long Id { get; set; }

        public Breed Breed { get; set; }

        public DateTime? HatchingDate { get; set; }

        public Colors? MarkingColor { get; set; }

        public bool IsFertilized { get; set; }

        public DateTime? BroodStartDate { get; set; }

        public QueenState State { get; set; }

        public long FarmId { get; set; }

        [ForeignKey("FarmId")]
        public Farm Farm { get; set; }

        public virtual ICollection<BeefamilyQueen> BeeFamilyQueens { get; set; }
        public virtual ICollection<QueensRaising> QueensRaisings { get; set; }
    }
}