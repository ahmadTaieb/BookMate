using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMate.Entities
{
    public class BookFavorite
    {


        public int Id { get; set; }

        [ForeignKey("Book")]
        public Guid BookId { get; set; }
        public Book? Book { get; set; }

        [ForeignKey("Favorite")]
        public int FavoriteId { get; set; }
        public Favorite? Favorite { get; set; }
    }
}
