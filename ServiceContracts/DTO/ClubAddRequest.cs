using BookMate.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts.DTO
{
    public class ClubAddRequest
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public bool? Hidden { get; set; } = false;
        public IFormFile? ImageFile { get; set; }
        public string? ImageUrl {  get; set; }
        public string? ApplicationUserId { get; set; }
        public ApplicationUser? ApplicationUser { get; set; }
    }
}
