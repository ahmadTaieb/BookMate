using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMate.Entities
{
    public class Favorite
    {
        public int Id{ get; set; }   

        [ForeignKey("UserLibrary")]
        public string UserId { get; set; }
        public ApplicationUser user { get; set; }

        public List<BookFavorite>? books { get; set; } = new List<BookFavorite>();


    }
}
