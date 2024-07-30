using BookMate.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts.DTO
{
    public class ReactAddRequest
    {
        public Reaction Reaction { get; set; }
        public string? ApplicationUserId { get; set; }
        public Guid? PostId { get; set; }
    }
}
