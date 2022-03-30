using BeekeepingApi.DTOs.BeeFamilyDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeekeepingApi.DTOs.ApiaryBeeFamilyDTOs
{
    public class ApiaryBeeFamilyReadForApiaryDTO
    {
        public long Id { get; set; }

        public DateTime ArriveDate { get; set; }

        public long ApiaryId { get; set; }

        public long BeeFamilyId { get; set; }

        public BeeFamilyReadDTO BeeFamily { get; set; }
    }
}
