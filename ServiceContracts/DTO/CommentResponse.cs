using BookMate.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts.DTO
{
    public class CommentResponse
    {
        public string Id { get; set; }
        public string Content { get; set; }
        public Guid? PostId { get; set; }
        public string? ApplicationUserName { get; set; }
        public string? ApplicationUserId { get; set; }
    }
}
