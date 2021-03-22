using BeekeepingApi.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BeekeepingApi.DTOs.TodoItemDTOs
{
    public class TodoItemCreateDTO
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public TodoItemPriority Priority { get; set; }

        public string Description { get; set; }

        [Column(TypeName = "date")]
        [DisplayFormat(DataFormatString = "{yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DueDate { get; set; }

        [Required]
        public bool IsComplete { get; set; }

        public long FarmId { get; set; }
        public long? ApiaryId { get; set; }
        public long? BeehiveId { get; set; }
    }
}
