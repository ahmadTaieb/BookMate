using BookMate.DataAccess.IRepository;
using BookMate.Entities;
using Microsoft.AspNetCore.Identity;
using ServiceContracts;
using ServiceContracts.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class CommentService : ICommentService
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private IUnitOfWork _unitOfWork;
        private IPostService _postService;



        public CommentService(UserManager<ApplicationUser> userManager, IUnitOfWork unitOfWork, IPostService postService)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _postService = postService;
        }
        public async Task<Comment> CreateAsync(CommentAddRequest commentAddRequest)
        {
            Comment comment = new Comment
            {
                Content = commentAddRequest.Content,
                ApplicationUserId = commentAddRequest.ApplicationUserId,
                PostId = commentAddRequest.PostId,
            };
            await _unitOfWork.Comment.Add(comment);
            _unitOfWork.saveAsync();
            return comment;
        }

        public async Task<Comment> DeleteAsync(Guid commentId)
        {
            Comment comment = await GetAsync(commentId);
            await _unitOfWork.Comment.Delete(comment);
            _unitOfWork.saveAsync();
            return comment;
        }

        public async Task<List<Comment>> GetAllAsync(Guid postId)
        {
            var comments = await _unitOfWork.Comment.GetAll(postId);
            foreach (var c in comments)
            {
                if (c == null)
                    comments.Remove(c);
            }
            return comments;
        }

        public Task<Comment> GetAsync(Guid commentId)
        {
            return _unitOfWork.Comment.Get(commentId);
        }

        public async Task<Comment> UpdateAsync(Guid id, CommentAddRequest commentUpdateRequest)
        {
            Comment c = await _unitOfWork.Comment.Update(id, commentUpdateRequest.Content);
            _unitOfWork.saveAsync();
            return c;
        }

        public async Task<bool> isAuthor(string userId, Guid commentId)
        {
            Comment comment = await GetAsync(commentId);
            if (comment.Id != commentId)
                return false;
            return true;

        }
    }
}
