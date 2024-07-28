﻿using BookMate.DataAccess.Data;
using BookMate.DataAccess.IRepository;
using BookMate.Entities;
using Microsoft.EntityFrameworkCore;
using ServiceContracts.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMate.DataAccess.Repository
{
    public class PostRepository : IPostRepository
    {
        private ApplicationDbContext _db;

        public PostRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<Post> Add(Post post)
        {
            _db.Posts.Add(post);
            
            return  post;
        }

        public async Task<bool> Delete(Post post)
        {
            _db.Posts.Remove(post);
            return true;
        }

        public async Task<Post> Get(Guid id)
        {
            return await _db.Posts.FirstOrDefaultAsync(p => p.Id == id);

        }

        public async Task<List<Post>> GetAll(Guid clubId)
        {
            var posts = await _db.Posts.Where(i => i.ClubId == clubId).ToListAsync();
            return posts;
        }

        public async Task<Post> Update(Guid id, PostAddRequest post)
        {
            Post? matchingPost = await _db.Posts.FirstOrDefaultAsync(q => q.Id == id);
            if (matchingPost != null)
            {
                matchingPost.Content = post.Content;
                matchingPost.ImageUrl = post.ImageUrl;

                _db.Posts.Update(matchingPost);
            }
            return matchingPost;
        }
    }
}
