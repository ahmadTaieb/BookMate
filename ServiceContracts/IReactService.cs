using BookMate.Entities;
using ServiceContracts.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts
{
    public interface IReactService
    {
        Task<React> CreateAsync(ReactAddRequest reactAddRequest);
        Task<React> UpdateAsync(React react, ReactAddRequest reactAddRequest);
        Task<React> DeleteAsync(Guid id);
        Task<React> GetAsync(string userId,Guid postId);
        Task<List<React>> GetAllAsync(Guid postId);
        Task<List<ReactResponse>> GetAllResponseAsync(Guid postId);
        Task<int[]> GetCountAsync(Guid postId);
        Task<React> CheckIfReact(string userId, string postId);

    }
}
