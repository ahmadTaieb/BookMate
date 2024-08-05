using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMate.Entities
{
    public class Notification
    {
        [Key]
        public Guid Id { get; set; }
        public string? Message { get; set; }
        public DateTime? Date { get; set; }
        public bool IsRead { get; set; } = false;
        public string? ApplicationUserId { get; set; }
    }
}
