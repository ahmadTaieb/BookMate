using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts.DTO
{
    public class TopCategoriesResponse
    {

        public TopCategoriesResponse(string name, int cnt)
        {
            CategoryName = name;
            Count = cnt;
        }

        public string? CategoryName { get; set; }
        public int? Count { get; set; }
    }
}
