using BeekeepingApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeekeepingApi.DTOs.BeehiveComponentDTOs
{
    public class BeehiveComponentReadDTO
    {
        public long Id { get; set; }
        public ComponentTypes Type { get; set; }
        public int? Position { get; set; }
        public DateTime InstallationDate { get; set; }
        public long BeehiveId { get; set; }
    }
}
