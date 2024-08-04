using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMate.Entities
{
    public class Post
    {
        [Key]
        public Guid Id { get; set; }
        public string? Content { get; set; }
        public string? ImageUrl { get; set; }
        public ICollection<Comment>? Comments { get; set; }
        public ICollection<React>? Reacts { get; set; }
        public string? ApplicationUserId { get; set; }
        public ApplicationUser? ApplicationUser { get; set; }
        public Guid? ClubId { get; set; }
        public Club? Club { get; set; }
    }
}
