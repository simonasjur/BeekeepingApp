using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BeekeepingApi.Helpers
{
    public class ChangePasswordModel
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        [MinLength(6)]
        public string NewPassword { get; set; }
    }
}
