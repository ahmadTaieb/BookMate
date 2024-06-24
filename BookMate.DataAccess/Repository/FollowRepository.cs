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
            _db.ApplicationUserRelations.Add(entity);
            return entity;
        }

        public Task<ApplicationUserRelation> Delete(ApplicationUserRelation entity)
        {
            throw new NotImplementedException();
        }

        public Task<List<ApplicationUserRelation>> GetAllFollowers(string id)
        {
            throw new NotImplementedException();
        }

        public Task<List<ApplicationUserRelation>> GetAllFollowing(string id)
        {
            throw new NotImplementedException();
        }

        public Task<ApplicationUserRelation> Update(ApplicationUserRelation entity)
        {
            throw new NotImplementedException();
        }

        public List<ApplicationUserRelation> GetAllFollowersRequests(string id)
        {
            var users = _db.ApplicationUsers
                .Include(x => x.Followers)
                .ThenInclude(y => y.ApplicationUserChild)
                .FirstOrDefault(z => z.Id == id);

            return (List<ApplicationUserRelation>)users.Followers;
        }
    }
}
