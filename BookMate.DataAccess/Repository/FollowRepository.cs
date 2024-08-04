using BookMate.DataAccess.Data;
using BookMate.DataAccess.IRepository;
using BookMate.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMate.DataAccess.Repository
{
    public class FollowRepository : IFollowRepository
    {
        private ApplicationDbContext _db;

        public FollowRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<ApplicationUserRelation> Add(ApplicationUserRelation entity)
        {
            var e = new ApplicationUserRelation
            {
                ApplicationUserParentId = entity.ApplicationUserParentId,
                ApplicationUserChildId = entity.ApplicationUserChildId,
                //ApplicationUserParent = _db.ApplicationUsers.FirstOrDefault(i => i.Id == entity.ApplicationUserParentId),
                //ApplicationUserChild = _db.ApplicationUsers.FirstOrDefault(i => i.Id == entity.ApplicationUserChildId),
            };
            _db.ApplicationUserRelations.Add(e);
            return entity;
        }

        public async Task<bool> Delete(ApplicationUserRelation entity)
        {
            _db.ApplicationUserRelations.Remove(entity);
            return true;
        }

        public Task<List<ApplicationUserRelation>> GetAllFollowers(string id)
        {
            throw new NotImplementedException();
        }

        public List<ApplicationUser> GetAllFollowing(string id)
        {
            return _db.ApplicationUserRelations
            .Where(r => r.ApplicationUserParentId == id)
            .Include(r => r.ApplicationUserChild)
            .Select(r => r.ApplicationUserChild)
            .ToList();
        }

        public Task<ApplicationUserRelation> Update(ApplicationUserRelation entity)
        {
            throw new NotImplementedException();
        }

        public List<ApplicationUser> GetAllFollowersRequests(string id)
        {
            return _db.ApplicationUserRelations
            .Where(r => r.ApplicationUserChildId == id)
            .Include(r => r.ApplicationUserParent)
            .Select(r => r.ApplicationUserParent)
            .ToList();
        }

        public ApplicationUserRelation Get(string parentId, string childId)
        {
            ApplicationUserRelation x = _db.ApplicationUserRelations.Where(i => i.ApplicationUserParentId == parentId).FirstOrDefault(j => j.ApplicationUserChildId == childId);
            return x;
        }
    }
}
