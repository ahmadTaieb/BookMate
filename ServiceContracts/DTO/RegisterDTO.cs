
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts.DTO
{
    public class RegisterDTO
    {
        public string Name { get; set; }

        public string Email { get; set; }
        
        public string Password { get; set; }
        public string? gender { get; set; }
        public int? Age { get; set; }
        public IFormFile? ImageFile { get; set; }
        public string? ImageUrl { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public DateTime? RegisteredAt { get; set; }
    }
}
