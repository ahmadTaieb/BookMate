using BookMate.DataAccess.IRepository;
using BookMate.Entities;
using BookMate.Entities.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.Identity.Client;
using ServiceContracts;
using ServiceContracts.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class ReactService : IReactService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private IUnitOfWork _unitOfWork;
        private IPostService _postService;
        private ICommentService _commentService;
        public ReactService(UserManager<ApplicationUser> userManager, IUnitOfWork unitOfWork, IPostService postService, ICommentService commentService)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _postService = postService;
            _commentService = commentService;
        }
        public async Task<React> CreateAsync(ReactAddRequest reactAddRequest)
        {
            React react = new React
            {
                Reaction = reactAddRequest.Reaction,
                ApplicationUserId = reactAddRequest.ApplicationUserId,
                PostId = reactAddRequest.PostId,
            };
            await _unitOfWork.React.Add(react);
            _unitOfWork.save();
            return react;
        }

        public async Task<React> DeleteAsync(Guid id)
        {
            React react =await _unitOfWork.React.Get(id);
            await _unitOfWork.React.Delete(react);
            _unitOfWork.save();
            return react;
        }

        public async Task<List<React>> GetAllAsync(Guid postId)
        {
            List<React> reactList = await _unitOfWork.React.GetAll(postId);
            return reactList;
        }

        public async Task<List<ReactResponse>> GetAllResponseAsync(Guid postId)
        {
            List<React> reactList = await _unitOfWork.React.GetAll(postId);
            List<ReactResponse> list = new List<ReactResponse>();
            foreach (React react in reactList)
            {
                ReactResponse reactResponse = new ReactResponse
                {
                    Reaction = react.Reaction,
                    ApplicationUserId = react.ApplicationUserId
                };
                list.Add(reactResponse);
            }
            return list;
        }

        public async Task<React> GetAsync(string userId, Guid postId)
        {
            React react =await _unitOfWork.React.GetByPostAndUser(userId, postId);
            return react;
        }

        public async Task<int[]> GetCountAsync(Guid postId)
        {
            List<React> reacts =await GetAllAsync(postId);
            int[] arr = { 0, 0, 0, 0 };
            foreach (React react in reacts)
            {
                if(react.Reaction == Reaction.Like)
                    arr[0]++;
                else if (react.Reaction == Reaction.Love)
                    arr[1]++;
                else if (react.Reaction == Reaction.Laugh)
                    arr[2]++;
                else if (react.Reaction == Reaction.Sad)
                    arr[3]++;

            }
            return arr;
        }

        public async Task<React> UpdateAsync(React react, ReactAddRequest reactAddRequest)
        {
            React r =await _unitOfWork.React.Update(react, reactAddRequest.Reaction);
            _unitOfWork.save();
            return r;
        }

        
    }
}
