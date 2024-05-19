using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMate.Entities
{
    public class Library
    {


        public int LibraryId { get; set; }


        [ForeignKey("UserLibrary")]
        public string UserId { get; set; }
        public ApplicationUser user { get; set;}

        public List<BookLibrary>? books { get; set; } = new List<BookLibrary>();

    }
}
