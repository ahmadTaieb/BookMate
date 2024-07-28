using ServiceContracts.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts
{
    public interface IFavoritesService
    {
        Task CreateFavorite(string userId);

        Task AddBookToFav(string userId, Guid bookId);

        Task RemoveBookFromFav (string userId, Guid bookId);

        Task<List<BookResponse?>?> GetFavoriteBooks(string userId);

    }
}
