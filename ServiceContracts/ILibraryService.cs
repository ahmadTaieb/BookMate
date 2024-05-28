using BookMate.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts
{
    public interface ILibraryService
    {
        Task CreateLibrary(string userId);
        //Task AddBookToLibrary(string userId, Guid bookId,ReadingStatus status);
        Task AddBookToLibrary(string userId, Guid bookId, ReadingStatus? readingStatus);
    }
}
