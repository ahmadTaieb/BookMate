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
    public class ApplicationUserRepository : IApplicationUserRepository
    {

        private ApplicationDbContext _db;

        public ApplicationUserRepository(ApplicationDbContext db) 
        {
            _db = db;
        }
        public void Add(ApplicationUser user)
        {
            _db.ApplicationUsers.Add(user);
        }
    }
}
