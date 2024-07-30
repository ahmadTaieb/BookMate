using BookMate.DataAccess.IRepository;
using BookMate.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using ServiceContracts;
using ServiceContracts.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class PostService : IPostService
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private IUnitOfWork _unitOfWork;
        


        public PostService(UserManager<ApplicationUser> userManager,   IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork; 
        }
        public async Task<Post> CreateAsync(PostAddRequest postAddRequest)
        {
            Post post = new Post
            {
                Content = postAddRequest.Content,
                ImageUrl = postAddRequest.ImageUrl,
                ApplicationUserId = postAddRequest.ApplicationUserId,
                //ApplicationUser = await _userManager.FindByIdAsync(postAddRequest.ApplicationUserId),
                ClubId = postAddRequest.ClubId,
                //Club =await _unitOfWork.Club.Get(postAddRequest.ClubId.ToString()),

            };
            await _unitOfWork.Post.Add(post);
            _unitOfWork.save();
            return post;
        }

        public async Task<Post> DeleteAsync(Guid postId)
        {
            Post post = await GetAsync(postId);
            await _unitOfWork.Post.Delete(post);
            _unitOfWork.save();
            return post;
        }

        public async Task<List<Post>> GetAllAsync(Guid clubId)
        {
            var posts = await _unitOfWork.Post.GetAll(clubId);
            foreach(var p in posts )
            {
                if(p == null) 
                    posts.Remove(p);
            }
            return posts;
        }

        public async Task<Post> GetAsync(Guid postId)
        {
            return await _unitOfWork.Post.Get(postId);
        }

        public async Task<Post> UpdateAsync(Guid id, PostAddRequest postUpdateRequest)
        {
            Post p =await _unitOfWork.Post.Update(id, postUpdateRequest);
            _unitOfWork.save();
            return p;
            
        }
    }
}
