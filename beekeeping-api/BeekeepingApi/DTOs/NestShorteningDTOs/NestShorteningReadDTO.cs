using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeekeepingApi.DTOs.NestShorteningDTOs
{
    public class NestShorteningReadDTO
    {
        public long Id { get; set; }

        public DateTime Date { get; set; }

        public int StayedCombs { get; set; }

        public double StayedHoney { get; set; }

        public int StayedBroodCombs { get; set; }

        public long BeehiveId { get; set; }
    }
}
