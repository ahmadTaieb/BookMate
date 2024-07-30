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
    public class ApplicationUserRepository : IApplicationUserRepository
    {

        private ApplicationDbContext _db;

        public ApplicationUserRepository(ApplicationDbContext db) 
        {
            _db = db;
        }

        public async Task<ApplicationUser> Add(ApplicationUser user)
        {
            _db.ApplicationUsers.Add(user);
            
            return user;

        }

        public async Task<bool> Delete(ApplicationUser user)
        {
            var relation1 = _db.ApplicationUserRelations.Where(x => x.ApplicationUserParentId == user.Id);
            var relation2 = _db.ApplicationUserRelations.Where(x => x.ApplicationUserChildId == user.Id);

            _db.ApplicationUserRelations.RemoveRange(relation1);
            _db.ApplicationUserRelations.RemoveRange(relation2);
            _db.ApplicationUsers.Remove(user);
            _db.SaveChanges(); 
            return true;

        }

        public async Task<ApplicationUser> Get(string id)
        {
            return await _db.ApplicationUsers.FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<List<ApplicationUser>> GetAll()
        {
            return await _db.ApplicationUsers.ToListAsync();
        }

        public async Task<ApplicationUser> Update(string id, ApplicationUserUpdateRequest user)
        {
            ApplicationUser? matchingUser = await _db.ApplicationUsers.FirstOrDefaultAsync(t => t.Id.Equals(id));

            if (matchingUser == null)
            {
                return null;
            }
            matchingUser.Name = user.Name ?? matchingUser.Name;
            matchingUser.Email = user.Email ?? matchingUser.Email;
            matchingUser.UserName = matchingUser.Email;
            matchingUser.gender = user.gender ?? matchingUser.gender;
            matchingUser.DateOfBirth = user.DateOfBirth ?? matchingUser.DateOfBirth;
            //matchingUser.RefreshTokens = user.RefreshTokens ?? matchingUser.RefreshTokens;


            return matchingUser;

        }
    }
}
