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
        Task<bool> Delete(ApplicationUserRelation entity);
        List<ApplicationUser> GetAllFollowersRequests(string id);
        Task<List<ApplicationUserRelation>> GetAllFollowers(string id);
        List<ApplicationUser> GetAllFollowing(string id);
        ApplicationUserRelation Get(string parentId, string childId);


    }
}
