using BookMate.DataAccess.IRepository;
using BookMate.Entities;
using ServiceContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class FollowService : IFollowService
    {
        private IUnitOfWork _unitOfWork;

        public FollowService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ApplicationUserRelation> FollowAsync(ApplicationUserRelation entity)
        {
            _unitOfWork.Follow.Add(entity);
            _unitOfWork.saveAsync();
            return entity;
        }
        public async Task<ApplicationUserRelation> UnFollowAsync(ApplicationUserRelation entity)
        {
            _unitOfWork.Follow.Delete(entity);
            _unitOfWork.saveAsync();
            return entity;
        }

        public async Task<List<ApplicationUser>> GetFollowRequestsAsync(string id)
        {
            var x = _unitOfWork.Follow.GetAllFollowersRequests(id);
            //List<ApplicationUser> users = new List<ApplicationUser>();

            //foreach (ApplicationUserRelation c in x)
            //{
            //    users.Add(c.ApplicationUserChild);
            //}

            return x;
        }
        public async Task<List<ApplicationUser>> GetFollowingAsync(string id)
        {
            var x = _unitOfWork.Follow.GetAllFollowing(id);
            //List<ApplicationUser> users = new List<ApplicationUser>();

            //foreach (ApplicationUserRelation c in x)
            //{
            //    users.Add(c.ApplicationUserChild);
            //}

            return x;
        }

        public async Task<bool> IsFollowing(ApplicationUserRelation entity)
        {
            var x = _unitOfWork.Follow.Get(entity.ApplicationUserParentId,entity.ApplicationUserChildId);
            if (x == null)
                return false;
            return true;
        }
    }
}
