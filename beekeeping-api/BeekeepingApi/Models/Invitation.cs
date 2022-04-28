using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BeekeepingApi.Models
{
    public class Invitation
    {
        [Key]
        public long Id { get; set; }

        public Guid Code { get; set; }

        public DateTime ExpirationDate { get; set; }

        public long FarmId { get; set; }
        [ForeignKey("FarmId")]
        public Farm Farm { get; set; }

    }
}
