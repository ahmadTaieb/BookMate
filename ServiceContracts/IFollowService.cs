using BookMate.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts
{
    public interface IFollowService
    {

        Task<ApplicationUserRelation> FollowAsync(ApplicationUserRelation entity);
        Task<ApplicationUserRelation> UnFollowAsync(ApplicationUserRelation entity);
        Task<List<ApplicationUser>> GetFollowRequestsAsync(string id);
        Task<List<ApplicationUser>> GetFollowingAsync(string id);
        Task<bool> IsFollowing(ApplicationUserRelation entity);
    }
}
