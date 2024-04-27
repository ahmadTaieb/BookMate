
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts.DTO
{
    public class RegisterDTO
    {
        public string Name { get; set; }
        //[Required]
        public string Email { get; set; }
        public string Password { get; set; }
        public string? gender { get; set; }
        public int? Age { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public DateTime? RegisteredAt { get; set; }
    }
}
