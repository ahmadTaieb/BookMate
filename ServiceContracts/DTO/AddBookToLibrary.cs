using BookMate.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts.DTO
{
    public class AddBookToLibrary
    {
        public Guid bookId { get; set; }

        public string? status { get; set; }
    }
}
