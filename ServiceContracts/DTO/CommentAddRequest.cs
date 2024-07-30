using BookMate.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts.DTO
{
    public class CommentAddRequest
    {
        public string? Content { get; set; }
        public Guid PostId { get; set; }
        public string? ApplicationUserId { get; set; }
    }
}
