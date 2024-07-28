using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts.DTO
{
    public class PostAddRequest
    {
        public string? Content { get; set; }
        public string? ImageUrl { get; set; }
        public string? ApplicationUserId { get; set; }
        public Guid? ClubId { get; set; }
    }
}
