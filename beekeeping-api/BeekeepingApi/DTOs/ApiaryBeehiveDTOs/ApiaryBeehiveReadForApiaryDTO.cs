using BeekeepingApi.DTOs.BeehiveDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeekeepingApi.DTOs.ApiaryBeehiveDTOs
{
    public class ApiaryBeehiveReadForApiaryDTO
    {
        public long Id { get; set; }

        public DateTime ArriveDate { get; set; }

        public double X { get; set; }

        public double Y { get; set; }

        public long ApiaryId { get; set; }

        public long BeehiveId { get; set; }

        public BeehiveReadDTO Beehive { get; set; }
    }
}
