using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BeekeepingApi.DTOs.FeedingDTOs
{
    public class FeedingEditDTO
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public DateTime? Date { get; set; }

        [Required]
        public double? Quantity { get; set; }

        public long FoodId { get; set; }
    }
}
