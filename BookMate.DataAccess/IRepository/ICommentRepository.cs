using BookMate.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMate.DataAccess.IRepository
{
    public interface ICommentRepository
    {

        Task<Comment> Add(Comment comment);
        Task<Comment> Update(Guid id,string comment);
        Task<Comment> Delete(Comment comment);
        Task<Comment> Get(Guid id);
        Task<List<Comment>> GetAll(Guid PostId);
    }
}
