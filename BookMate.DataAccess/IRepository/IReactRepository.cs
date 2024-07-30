using BookMate.Entities;
using BookMate.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMate.DataAccess.IRepository
{
    public interface IReactRepository
    {
        Task<React> Add(React react);
        Task<React> Update(React? matchingReact, Reaction reaction);
        Task<React> Delete(React react);
        Task<React> Get(Guid id);
        Task<List<React>> GetAll(Guid postId);
        Task<React> GetByPostAndUser(string userId, Guid postId);
        Task<List<ApplicationUser>> GetAllUsersReact(Guid postId);

    }
}
