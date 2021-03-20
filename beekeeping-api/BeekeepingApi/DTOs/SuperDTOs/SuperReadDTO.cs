using BeekeepingApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeekeepingApi.DTOs.SuperDTOs
{
    public class SuperReadDTO
    {
        public long Id { get; set; }
        public int Position { get; set; }
        public Colors Color { get; set; }
        public DateTime InstallationDate { get; set; }
        public long BeehiveId { get; set; }
    }
}
