using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMate.DataAccess.IRepository
{
    public interface IUnitOfWork
    {
        IApplicationUserRepository ApplicationUser { get; }
        IClubRepository Club { get; }
        IFollowRepository Follow { get; }
        IPostRepository Post { get; }
        ICommentRepository Comment { get; }
        IReactRepository React { get; }
        IReportRepository Report { get; }
        public void saveAsync();
        public void save();
    }
}
