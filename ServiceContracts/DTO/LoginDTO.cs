﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts.DTO
{
    public class LoginDTO
    {
        public LoginDTO() { }

        public string Email { get; set; }
        public string Password { get; set; }

    }
}
