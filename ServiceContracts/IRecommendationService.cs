using ServiceContracts.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts
{
    public interface IRecommendationService
    {

        Task AddFavoriteCategories(List<string> categories,string userId);

        Task<List<BookResponse>> RecommendationsBooks(string userId);
    }
}
