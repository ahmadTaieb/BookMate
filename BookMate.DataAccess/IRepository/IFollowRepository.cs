using BookMate.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMate.DataAccess.IRepository
{
    public interface IFollowRepository
    {
        Task<ApplicationUserRelation> Add(ApplicationUserRelation entity);
        Task<ApplicationUserRelation> Update(ApplicationUserRelation entity);
        Task<ApplicationUserRelation> Delete(ApplicationUserRelation entity);
        List<ApplicationUserRelation> GetAllFollowersRequests(string id);
        Task<List<ApplicationUserRelation>> GetAllFollowers(string id);
        Task<List<ApplicationUserRelation>> GetAllFollowing(string id);


    }
}
