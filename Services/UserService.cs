﻿using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Security.Cryptography;
using BookMate.Entities;
using ServiceContracts.DTO;
using ServiceContracts;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using BookMate.DataAccess.Repository;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Reflection.Metadata.Ecma335;
using BookMate.DataAccess.IRepository;
using Microsoft.AspNetCore.Http;

namespace Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private IUnitOfWork _unitOfWork;
        //private readonly JWT _jwt;
        private IConfiguration _configuration;

        public UserService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager,IConfiguration configuration,IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }

        public async Task<AuthModel> RegisterAsync(RegisterDTO model)
        {
            if (await _userManager.FindByEmailAsync(model.Email) is not null)
                return new AuthModel { Message = "Email is already registered!" };

            model.ImageUrl = await GetImageUrl(file: model.ImageFile);

            var user = new ApplicationUser
            {
                Name = model.Name,
                Email = model.Email,
                UserName = model.Email,
                gender = model.gender,
                //Age = model.Age,
                ImageUrl = model.ImageUrl,
                DateOfBirth = model.DateOfBirth,
                
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                var errors = string.Empty;

                foreach (var error in result.Errors)
                    errors += $"{error.Description},";

                return new AuthModel { Message = errors };
            }

            //await _userManager.AddToRoleAsync(user, "User");

            var jwtSecurityToken = await CreateJwtToken(user);

            var refreshToken = GenerateRefreshToken();
            //user.RefreshTokens?.Add(refreshToken);
            await _userManager.UpdateAsync(user);

            return new AuthModel
            {
                Email = user.Email,
                //ExpiresOn = jwtSecurityToken.ValidTo,
                IsAuthenticated = true,
                Roles = new List<string> { "User" },
                Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                Username = user.UserName,
                Id=user.Id,
                //RefreshToken = refreshToken.Token,
                //RefreshTokenExpiration = refreshToken.ExpiresOn
            };
        }

        public async Task<AuthModel> GetTokenAsync(TokenRequest model)
        {
            var authModel = new AuthModel();

            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user is null || !await _userManager.CheckPasswordAsync(user, model.Password))
            {
                authModel.Message = "Email or Password is incorrect!";
                //authModel.RefreshTokenExpiration = DateTime.UtcNow;
                return authModel;
            }

            var jwtSecurityToken = await CreateJwtToken(user);
            var rolesList = await _userManager.GetRolesAsync(user);

            authModel.IsAuthenticated = true;
            authModel.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            authModel.Email = user.Email;
            authModel.Username = user.UserName;
            //authModel.ExpiresOn = jwtSecurityToken.ValidTo;
            authModel.Roles = rolesList.ToList();

            //if (user.RefreshTokens.Any(t => t.IsActive))
            //{
            //    var activeRefreshToken = user.RefreshTokens.FirstOrDefault(t => t.IsActive);
            //    authModel.RefreshToken = activeRefreshToken.Token;
            //    authModel.RefreshTokenExpiration = activeRefreshToken.ExpiresOn;
            //}
            //else
            //{
            //    var refreshToken = GenerateRefreshToken();
            //    authModel.RefreshToken = refreshToken.Token;
            //    authModel.RefreshTokenExpiration = refreshToken.ExpiresOn;
            //    user.RefreshTokens.Add(refreshToken);
            //    await _userManager.UpdateAsync(user);
            //}

            return authModel;
        }

        public async Task<string> AddRoleAsync(string userId,string role)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user is null || !await _roleManager.RoleExistsAsync(role))
                return "Invalid user ID or Role";

            if (await _userManager.IsInRoleAsync(user, role))
                return "User already assigned to this role";

            var result = await _userManager.AddToRoleAsync(user, role);

            return result.Succeeded ? string.Empty : "Sonething went wrong";
        }

        private async Task<JwtSecurityToken> CreateJwtToken(ApplicationUser user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);
            var roleClaims = new List<Claim>();

            foreach (var role in roles)
                roleClaims.Add(new Claim("roles", role));

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Name),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("uid", user.Id)
            }
            .Union(userClaims)
            .Union(roleClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]!));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _configuration["JWT:Issuer"],
                audience: _configuration["JWT:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(60),
                signingCredentials: signingCredentials);

            return jwtSecurityToken;
        }

        //public async Task<AuthModel> RefreshTokenAsync(string token)
        //{
        //    var authModel = new AuthModel();

        //    var user =  _userManager.Users.SingleOrDefault(u => u.RefreshTokens.Any(t => t.Token == token));

        //    if (user == null)
        //    {
        //        authModel.Message = "Invalid token";
        //        return authModel;
        //    }

        //    var refreshToken = user.RefreshTokens.Single(t => t.Token == token);

        //    if (!refreshToken.IsActive)
        //    {
        //        authModel.Message = "Inactive token";
        //        return authModel;
        //    }

        //    refreshToken.RevokedOn = DateTime.UtcNow;

        //    var newRefreshToken = GenerateRefreshToken();
        //    user.RefreshTokens.Add(newRefreshToken);
        //    await _userManager.UpdateAsync(user);

        //    var jwtToken = await CreateJwtToken(user);
        //    authModel.IsAuthenticated = true;
        //    authModel.Token = new JwtSecurityTokenHandler().WriteToken(jwtToken);
        //    authModel.Email = user.Email;
        //    authModel.Username = user.UserName;
        //    var roles = await _userManager.GetRolesAsync(user);
        //    authModel.Roles = roles.ToList();
        //    //authModel.RefreshToken = newRefreshToken.Token;
        //    //authModel.RefreshTokenExpiration = newRefreshToken.ExpiresOn;

        //    return authModel;
        //}

        //public async Task<bool> RevokeTokenAsync(string token)
        //{
        //    var user =  _userManager.Users.SingleOrDefault(u => u.RefreshTokens.Any(t => t.Token == token));

        //    if (user == null)
        //        return false;

        //    var refreshToken = user.RefreshTokens.Single(t => t.Token == token);

        //    if (!refreshToken.IsActive)
        //        return false;

        //    refreshToken.RevokedOn = DateTime.UtcNow;

        //    await _userManager.UpdateAsync(user);

        //    return true;
        //}

        private RefreshToken GenerateRefreshToken()
        {
            var randomNumber = new byte[32];

            using var generator = new RNGCryptoServiceProvider();

            generator.GetBytes(randomNumber);

            return new RefreshToken
            {
                Token = Convert.ToBase64String(randomNumber),
                ExpiresOn = DateTime.UtcNow.AddDays(10),
                CreatedOn = DateTime.UtcNow
            };
        }

        public async Task<ApplicationUserUpdateRequest> UpdateUserAsync(ApplicationUser matchingUser, ApplicationUserUpdateRequest user)
        {
            user.ImageUrl = await GetImageUrl(file: user.ImageFile);


            matchingUser.Name = user.Name ?? matchingUser.Name;
            matchingUser.Email = user.Email ?? matchingUser.Email;
            matchingUser.UserName = matchingUser.Email;
            matchingUser.gender = user.gender ?? matchingUser.gender;
            matchingUser.DateOfBirth = user.DateOfBirth ?? matchingUser.DateOfBirth;
            matchingUser.ImageUrl = user.ImageUrl ?? matchingUser.ImageUrl;


          
            await _userManager.UpdateAsync(matchingUser);
            return user;
        }

        public async Task<ApplicationUser> DeleteUserAsync(ApplicationUser user)
        {
            //_userManager.DeleteAsync(user);
            await _unitOfWork.ApplicationUser.Delete(user);
           // _unitOfWork.save();
            return user;
        }

        public Task<List<ApplicationUser>> GetAllUsersAsync()
        {

            return _unitOfWork.ApplicationUser.GetAll();
        }
        public Task<ApplicationUser> GetUserAsync(string id)
        {
            var user = _userManager.FindByIdAsync(id);
            return user;
        }

        private async Task<string?> GetImageUrl(IFormFile? file)
        {
            if (file == null)
            {
                return null;
            }

            string filename = "";
            try
            {
                var extension = Path.GetExtension(file.FileName);
                filename = DateTime.Now.Ticks.ToString() + extension;

                // Save images to wwwroot/images
                var relativePath = Path.Combine("wwwroot", "images", "users");

                // Combine the relative path with the current directory
                var filepath = Path.Combine(Directory.GetCurrentDirectory(), relativePath);

                if (!Directory.Exists(filepath))
                {
                    Directory.CreateDirectory(filepath);
                }

                var exactpath = Path.Combine(filepath, filename);
                using (var stream = new FileStream(exactpath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
            }
            catch (Exception ex)
            {
                // Handle the exception (e.g., log it)
                Console.WriteLine(ex.Message);
            }
            return $"/images/users/{filename}"; // Return the relative URL path
        }


    }
}