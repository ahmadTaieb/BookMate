using Microsoft.AspNetCore.Identity;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMate.Entities
{
    public class ApplicationUser : IdentityUser
    {

       
        [Required]
        public string Name { get; set; }
        public string? gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public DateTime? RegisteredAt { get; set; }
        //public List<RefreshToken>? RefreshTokens { get; set; }
        public string? ImageUrl { get; set; }
        public ICollection<Club>? Clubs { get; set; }
        public ICollection<ApplicationUserClub>? ClubsMember { get; set;} = new List<ApplicationUserClub>();
        public ICollection<ApplicationUserRelation>? Followers { get; set; } = new List<ApplicationUserRelation>();
        public ICollection<ApplicationUserRelation>? Following { get; set; } = new List<ApplicationUserRelation>();


    }
}
