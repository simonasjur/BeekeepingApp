using BeekeepingApi.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BeekeepingApi.DTOs.FarmWorkerDTOs
{
    public class FarmWorkerCreateDTO
    {
        [Required]
        public WorkerRole? Role { get; set; }
        public long UserId { get; set; }

        public long FarmId { get; set; }
    }
}
