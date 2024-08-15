using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts.DTO
{
    public class ResetPasswordModel
    {
        
        public string? Email { get; set; }

        public string? ResetCode { get; set; }
        public string? Token { get; set; }

    }
}

