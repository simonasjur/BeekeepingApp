using BeekeepingApi.DTOs.BeeFamilyDTO;
using BeekeepingApi.DTOs.QueenDTOs;
using BeekeepingApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeekeepingApi.DTOs.QueensRaisingDTOs
{
    public class QueensRaisingReadDTO
    {
        public long Id { get; set; }

        public DateTime StartDate { get; set; }

        public int LarvaCount { get; set; }

        public DevelopmentPlace DevelopmentPlace { get; set; }

        public int CappedCellCount { get; set; }

        public int QueensCount { get; set; }

        public long MotherId { get; set; }

        public long BeefamilyId { get; set; }

        public QueenReadDTO Mother { get; set; }

        public BeeFamilyReadDTO Beefamily { get; set; }
    }
}
