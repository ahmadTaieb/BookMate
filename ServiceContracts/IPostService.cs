using BookMate.Entities;
using ServiceContracts.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts
{
    public interface IPostService
    {
        Task<Post> CreateAsync(PostAddRequest postAddRequest);
        Task<Post> UpdateAsync(Guid id,PostAddRequest postUpdateRequest);
        Task<Post> DeleteAsync(Guid postId);
        Task<Post> GetAsync(Guid postId);
        Task<List<Post>> GetAllAsync(Guid clubId);
        Task<bool> isAuthor(string userId, Guid postId);
    }
}
