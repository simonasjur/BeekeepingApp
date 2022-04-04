using BeekeepingApi.DTOs.BeehiveDTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BeekeepingApi.DTOs.BeehiveBeeFamilyDTOs
{
    public class BeehiveBeeFamilyReadDTO
    {
        public long Id { get; set; }

        public DateTime ArriveDate { get; set; }

        public DateTime? DepartDate { get; set; }

        public long BeehiveId { get; set; }

        public long BeeFamilyId { get; set; }

        public BeehiveReadDTO Beehive { get; set; }
    }
}
