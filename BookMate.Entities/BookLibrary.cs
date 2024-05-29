using BookMate.Entities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMate.Entities
{
    public class BookLibrary
    {

        public int Id { get; set; }

        [ForeignKey("Book")]
        public Guid BookId { get; set; }

        public Book Book { get; set; }

       public ReadingStatus? ReadingStatus { get; set; }

        [ForeignKey("Library")]
        public int LibraryId { get; set; }
        public Library Library { get; set; } 

    }
}
