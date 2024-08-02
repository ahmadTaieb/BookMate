using ServiceContracts.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts
{
    public interface IFeedBackService
    {
         Task<List<CategoryPercentResponse?>?> CategoriesPercent();

        Task<List<BookResponse?>?> TopReadBooks();

        Task<List<ApplicationUserResponse?>?> TopReader();

    }
}
