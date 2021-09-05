using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BeekeepingApi.DTOs.UserDTOs
{
    public class UserEditDTO
    {
        [Key]
        public long Id { get; set; }

        [MinLength(6)]
        public string Password { get; set; }

        [Required]
        public long? DefaultFarmId { get; set; }
    }
}
