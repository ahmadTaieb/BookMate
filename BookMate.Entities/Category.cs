using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMate.Entities
{
    public class Category
    {
        [Key]
       public int categoryID { get; set; }
        [StringLength(30)]
        public string categoryName { get; set; }

        public ICollection<Book>? books { get; set; }



    }
}
