using BookMate.DataAccess.Data;
using BookMate.DataAccess.IRepository;
using BookMate.Entities;
using BookMate.Entities.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMate.DataAccess.Repository
{
    public class ReactRepository : IReactRepository
    {
        private ApplicationDbContext _db;

        public ReactRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<React> Add(React react)
        {
            _db.Reacts.Add(react);
            return react;
        }

        public async Task<React> Delete(React react)
        {
            _db.Reacts.Remove(react);
            return react;
        }

        public async Task<React> Get(Guid id)
        {
            React? react = _db.Reacts.FirstOrDefault(x => x.Id.ToString() == id.ToString());
            return react;

        }

        public async Task<React> GetByPostAndUser(string userId,Guid postId)
        {
            React? react =await _db.Reacts.FirstOrDefaultAsync(i => i.ApplicationUserId ==  userId && i.PostId == postId);
            return react;
        }

        public async Task<List<React>> GetAll(Guid postId)
        {

            List<React> reacts = _db.Reacts.Where(x => x.PostId == postId).ToList();
            return reacts;
        }
        public async Task<List<ApplicationUser>> GetAllUsersReact(Guid postId)
        {
            return _db.Reacts
            .Where(r => r.PostId == postId)
            .Include(r => r.ApplicationUser)
            .Select(r => r.ApplicationUser)
            .ToList();
        }


        public async Task<React> Update(React? matchingReact, Reaction reaction)
        {
            //React? matchingReact =await _db.Reacts.FirstOrDefaultAsync(q => q.Id.ToString() == id.ToString());
            if(matchingReact != null)
            {
                matchingReact.Reaction = reaction;
                _db.Reacts.Update(matchingReact);
            }
            return matchingReact;
        }
    }
}
