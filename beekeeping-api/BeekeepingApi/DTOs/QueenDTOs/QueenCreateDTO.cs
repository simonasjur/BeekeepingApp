using BeekeepingApi.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BeekeepingApi.DTOs.QueenDTOs
{
    public class QueenCreateDTO
    {
        [Required]
        public Breed? Breed { get; set; }

        public DateTime? HatchingDate { get; set; }

        public Colors? MarkingColor { get; set; }

        [Required]
        public bool? IsFertilized { get; set; }

        public DateTime? BroodStartDate { get; set; }

        [Required]
        public QueenState? State { get; set; }

        public long FarmId { get; set; }
    }
}
