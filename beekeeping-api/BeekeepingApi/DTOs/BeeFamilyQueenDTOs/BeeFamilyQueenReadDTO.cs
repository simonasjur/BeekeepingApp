using BeekeepingApi.DTOs.QueenDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeekeepingApi.DTOs.BeeFamilyQueenDTOs
{
    public class BeeFamilyQueenReadDTO
    {
        public long Id { get; set; }

        public DateTime InsertDate { get; set; }

        public DateTime? TakeOutDate { get; set; }

        public long QueenId { get; set; }

        public long BeeFamilyId { get; set; }

        public QueenReadDTO Queen { get; set; }
    }
}
