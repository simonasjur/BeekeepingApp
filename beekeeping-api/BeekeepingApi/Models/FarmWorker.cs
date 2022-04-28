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
        public string FirstName { get; set; }
        public string LastName { get; set; }


        // Beefamilies (apiaryBeefaimly, beefamilyQueens, beehiveBeefamily) - 0:1:2
        // Beehives - 3:4:5
        // BeehiveComponent - 6:7:8
        // Harvests - 9:10:11
        // NestExpansion - 12:13:14
        // NestReduction - 15:16:17
        // Queens - 18:19:20
        // QueensRaisings - 21:22:23
        // TodoItems - 24:25:26
        // Feedings - 27:28:29
        [Required]
        public string Permissions { get; set; }

        [Required]
        public WorkerRole Role { get; set; }

        [ForeignKey("UserID")]
        public long UserId { get; set; }

        public User User { get; set; }

        [ForeignKey("FarmID")]
        public long FarmId { get; set; }

        public Farm Farm { get; set; }
    }
}
