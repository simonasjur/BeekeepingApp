using BeekeepingApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeekeepingApi.DTOs.FarmWorkerDTOs
{
    public class FarmWorkerReadDTO
    {
        public long Id { get; set; }
        public WorkerRole? Role { get; set; }
        public long UserId { get; set; }
        public long FarmId { get; set; }
    }
}
