using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMate.Entities
{
    public class Report
    {
        [Key]
        public Guid Id { get; set; }
        public string? ApplicationUserId { get; set; }
        public Guid? PostId { get; set; }

    }
}
