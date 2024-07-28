using BookMate.Entities;
using ServiceContracts.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMate.DataAccess.IRepository
{
    public interface IPostRepository
    {
        Task<Post> Add(Post post);
        Task<Post> Update(Guid id,PostAddRequest post);
        Task<bool> Delete(Post post);
        Task<Post> Get(Guid id);
        Task<List<Post>> GetAll(Guid clubId);

    }
}
