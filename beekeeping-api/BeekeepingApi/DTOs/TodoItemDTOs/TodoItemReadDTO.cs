using BeekeepingApi.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BeekeepingApi.DTOs.TodoItemDTOs
{
    public class TodoItemReadDTO
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public TodoItemPriority Priority { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public bool IsComplete { get; set; }
        public long FarmId { get; set; }
        public long ApiaryId { get; set; }
        public long BeeFamilyId { get; set; }
    }
}
