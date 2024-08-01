using BookMate.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts.DTO
{
    public class ApplicationUserUpdateRequest
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? currentPassword { get; set; }
        public string? Password { get; set; }
        public string? gender { get; set; }
        public DateTime? DateOfBirth { get; set;}
        public IFormFile? ImageFile { get; set; }
        public string? ImageUrl { get; set; }
        public List<RefreshToken>? RefreshTokens { get; set; }
        public DateTime? RegisteredAt { get; set; }

        

    }
}
