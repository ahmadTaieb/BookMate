using BookMate.DataAccess.Data;
using BookMate.DataAccess.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMate.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext _db;
        public IApplicationUserRepository ApplicationUser {  get; private set; }
        public IClubRepository Club { get; private set; }
        public IFollowRepository Follow { get; private set; }

        public IPostRepository Post {  get; private set; }
        public ICommentRepository Comment { get; private set; }
        public IReactRepository React { get; private set; }
        public IReportRepository Report { get; private set; }

        public UnitOfWork (ApplicationDbContext db)
        {
            _db = db;
            ApplicationUser = new ApplicationUserRepository(_db);
            Club = new ClubRepository(_db);
            Follow = new FollowRepository(_db);
            Post = new PostRepository(_db);
            Comment = new CommentRepository(_db);
            React = new ReactRepository(_db);
            Report = new ReportRepository(_db);

        }

        public void saveAsync()
        {
            _db.SaveChangesAsync();
        }

        public void save()
        {
           _db.SaveChanges();
        }
    }
}
