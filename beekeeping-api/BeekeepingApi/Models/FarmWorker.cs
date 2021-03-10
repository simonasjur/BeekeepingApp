using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BeekeepingApi.Models
{
    public enum WorkerRole
    {
        Owner, Assistant
    }

    public class FarmWorker
    {
        [Required]
        public WorkerRole? Role { get; set; }

        public long UserId { get; set; }
        public User User { get; set; }

        public long FarmId { get; set; }
        public Farm Farm { get; set; }
    }
}
