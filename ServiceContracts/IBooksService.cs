using Microsoft.AspNetCore.Http;
using ServiceContracts.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts
{
    public interface IBooksService
    {
        BookResponse AddBook(BookAddRequest? bookAddRequest);
        List<BookResponse> GetAllBooks();
        BookResponse? GetBookByBookTitle(string? title);
        List<BookResponse> GetBooksByCategory(String? Category);


    }
}
