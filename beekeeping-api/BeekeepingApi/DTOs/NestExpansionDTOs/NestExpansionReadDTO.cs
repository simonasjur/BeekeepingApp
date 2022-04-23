using BeekeepingApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeekeepingApi.DTOs.NestExpansionDTOs
{
    public class NestExpansionReadDTO
    {
        public long Id { get; set; }

        public DateTime Date { get; set; }

        public FrameType FrameType { get; set; }

        public int CombSheets { get; set; }

        public int Combs { get; set; }

        public long BeefamilyId { get; set; }
    }
}
