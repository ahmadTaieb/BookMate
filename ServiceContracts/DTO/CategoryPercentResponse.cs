using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts.DTO
{
    public class CategoryPercentResponse
    {

        public CategoryPercentResponse(string name, double per)
        {
            CategoryName = name;
            Percent = per;
        }

        public string? CategoryName { get; set; }
        public double? Percent { get; set; }

    }
}
