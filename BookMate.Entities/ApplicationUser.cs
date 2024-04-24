using BookMate.DataAccess.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMate.Entities
{
    public class ApplicationUser
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public Gender gender { get; set; }
        public DateTime Created { get; set; }
        public DateTime DateOfBirth { get; set; }

    
    }
}
