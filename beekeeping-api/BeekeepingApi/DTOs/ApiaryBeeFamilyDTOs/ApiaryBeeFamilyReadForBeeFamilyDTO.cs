using BeekeepingApi.DTOs.ApiaryDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeekeepingApi.DTOs.ApiaryBeeFamilyDTOs
{
    public class ApiaryBeeFamilyReadForBeeFamilyDTO
    {
        public long Id { get; set; }

        public DateTime ArriveDate { get; set; }

        public DateTime? DepartDate { get; set; }

        public long BeeFamilyId { get; set; }

        public long ApiaryId { get; set; }

        public ApiaryReadDTO Apiary { get; set; }
    }
}
