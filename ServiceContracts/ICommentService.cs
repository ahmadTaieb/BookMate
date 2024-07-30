using BookMate.Entities;
using ServiceContracts.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts
{
    public interface ICommentService
    {
        Task<Comment> CreateAsync(CommentAddRequest commentAddRequest);
        Task<Comment> UpdateAsync(Guid id, CommentAddRequest commentUpdateRequest);
        Task<Comment> DeleteAsync(Guid commentId);
        Task<Comment> GetAsync(Guid commentId);
        Task<List<Comment>> GetAllAsync(Guid postId);
    }
}
