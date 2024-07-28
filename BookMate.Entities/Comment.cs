using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMate.Entities
{
    public class Comment
    {
        [Key]
        public Guid Id { get; set; }
        public string Content { get; set; }
        public Guid? PostId { get; set; }
        public Post? Post { get; set; }
        public string? ApplicationUserId { get; set; }
        public ApplicationUser? ApplicationUser { get; set; }


    }
}
