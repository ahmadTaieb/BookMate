using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMate.Entities
{
    public class Category
    {
       public int categoryID { get; set; }

        public string categoryName { get; set; }

        public ICollection<Book>? books { get; set; }



    }
}
