using BeekeepingApi.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BeekeepingApi.DTOs.QueenDTOs
{
    public class QueenEditDTO
    {
        [Key]
        public long Id { get; set; }

        public Breed Breed { get; set; }

        public DateTime? HatchingDate { get; set; }

        public Colors? MarkingColor { get; set; }

        public bool IsFertilized { get; set; }

        public DateTime? BroodStartDate { get; set; }

        public QueenState State { get; set; }
    }
}
