using BookMate.Entities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMate.Entities
{
    public class React
    {
        [Key]
        public Guid Id { get; set; }
        public string? ApplicationUserId { get; set; }
        public virtual ApplicationUser? ApplicationUser { get; set; }
        public Guid? PostId { get; set; }
        public virtual Post? Post { get; set; }
        public Reaction Reaction { get; set; }
    }
}
