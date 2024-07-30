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
        Task AddBook(BookAddRequest? bookAddRequest);
        List<BookResponse> GetAllBooks(string? userId);
        BookResponse? GetBookByBookTitle(string? title);
        //List<BookResponse> GetBooksByCategory(String? Category);
        Task<List<BookResponse?>>? GetBooksByCategory(List<string>categoriesName,string? userId);
        BookResponse? GetBookByBookId(Guid? Id);

        Task EditBookAsync(string Title, BookAddRequest? editedBook);

        Task DeleteBook(string title);
        
        Task<List<BookResponse?>> Search(string title);



    }
}
