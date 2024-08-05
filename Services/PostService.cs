using BookMate.DataAccess.IRepository;
using BookMate.Entities;
using Microsoft.AspNetCore.Http;
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
            postAddRequest.ImageUrl = await GetImageUrl(file: postAddRequest.ImageFile);

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
            postUpdateRequest.ImageUrl = await GetImageUrl(file: postUpdateRequest.ImageFile);
            Post p =await _unitOfWork.Post.Update(id, postUpdateRequest);
            _unitOfWork.save();
            return p;
            
        }

        public async Task<bool> isAuthor(string userId, Guid postId)
        {
            Post post =await GetAsync(postId);
            if(post.Id != postId)
                return false;
            return true;

        }

        private async Task<string?> GetImageUrl(IFormFile? file)
        {
            if (file == null)
            {
                return null;
            }

            string filename = "";
            try
            {
                var extension = Path.GetExtension(file.FileName);
                filename = DateTime.Now.Ticks.ToString() + extension;

                // Save images to wwwroot/images
                var relativePath = Path.Combine("wwwroot", "images", "posts");

                // Combine the relative path with the current directory
                var filepath = Path.Combine(Directory.GetCurrentDirectory(), relativePath);

                if (!Directory.Exists(filepath))
                {
                    Directory.CreateDirectory(filepath);
                }

                var exactpath = Path.Combine(filepath, filename);
                using (var stream = new FileStream(exactpath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
            }
            catch (Exception ex)
            {
                // Handle the exception (e.g., log it)
                Console.WriteLine(ex.Message);
            }
            return $"/images/posts/{filename}"; // Return the relative URL path
        }
    }
}
