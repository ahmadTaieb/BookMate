
using BookMate.Entities;
using ServiceContracts.DTO;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts
{
    public interface IUserService
    {
        Task<AuthModel> RegisterAsync(RegisterDTO model);
        Task<AuthModel> GetTokenAsync(TokenRequest model);
        Task<string> AddRoleAsync(string userId , string role);
        //Task<AuthModel> RefreshTokenAsync(string token);
        //Task<bool> RevokeTokenAsync(string token);
        Task<ApplicationUserUpdateRequest> UpdateUserAsync(string id, ApplicationUserUpdateRequest user);
        Task<ApplicationUser> DeleteUserAsync(ApplicationUser user);
        Task<List<ApplicationUser>> GetAllUsersAsync();



    }
}
