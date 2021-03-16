using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BeekeepingApi.DTOs.FarmDTOs
{
    public class FarmCreateDTO
    {
        [Required]
        public string Name { get; set; }

        [Column(TypeName = "date")]
        [DisplayFormat(DataFormatString = "{yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime CreationDate { get; set; }
    }
}
