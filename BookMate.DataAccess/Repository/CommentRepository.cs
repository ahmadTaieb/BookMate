using BookMate.DataAccess.Data;
using BookMate.DataAccess.IRepository;
using BookMate.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMate.DataAccess.Repository
{
    public class CommentRepository : ICommentRepository
    {
        private ApplicationDbContext _db;

        public CommentRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<Comment> Add(Comment comment)
        {
            _db.Comments.Add(comment);
            return comment;
        }

        public async Task<Comment> Delete(Comment comment)
        {
            var c = _db.Comments.Remove(comment);
            return comment;
        }

        public async Task<Comment> Get(Guid id)
        {
            var comment = _db.Comments.FirstOrDefault(c => c.Id == id);
            return comment;
        }

        public async Task<List<Comment>> GetAll(Guid PostId)
        {
            var commnets = _db.Comments.Where(i => i.PostId == PostId).ToList();
            return commnets;

        }

        public async Task<Comment> Update(Guid id,string comment)
        {
            var matchingComment =await Get(id);
            if (matchingComment != null)
            {
                matchingComment.Content = comment;
                _db.Comments.Update(matchingComment);
            }
            return matchingComment;
            
        }
    }
}
