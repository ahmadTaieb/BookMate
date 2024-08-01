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
    public class ApplicationUserAddRequest : IdentityUser
    {
        public string? Name { get; set; }
        
        public string? gender { get; set; }
        
        public DateTime? DateOfBirth { get; set; }
        public DateTime? RegisteredAt { get; set; }
        //public List<RefreshToken>? RefreshTokens { get; set; }

        public IFormFile? ImageFile { get; set; }

        public ApplicationUser ToApplicationUser()
        {
            return new ApplicationUser()
            {
                Name = Name,
                Email = Email,
                DateOfBirth = DateOfBirth,
                PasswordHash = PasswordHash,
                //RefreshTokens = RefreshTokens,
                RegisteredAt = RegisteredAt,
                gender = gender,

            };
        }
    }
}
