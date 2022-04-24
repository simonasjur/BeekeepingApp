using BeekeepingApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeekeepingApi.DTOs.FarmWorkerDTOs
{
    public class FarmWorkerReadDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }  
        public string Permissions { get; set; }
        public WorkerRole? Role { get; set; }
        public long UserId { get; set; }
        public long FarmId { get; set; }
    }
}
