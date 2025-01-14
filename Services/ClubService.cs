﻿using BookMate.DataAccess.IRepository;
using BookMate.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using ServiceContracts;
using ServiceContracts.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static System.Reflection.Metadata.BlobBuilder;

namespace Services
{
    public class ClubService : IClubService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private IUnitOfWork _unitOfWork;
        //private readonly JWT _jwt;
        private IConfiguration _configuration;


        public ClubService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }

        public async Task<Club> GetClub(string id)
        {
            return await _unitOfWork.Club.Get(id);
        }


        public async Task<Club> AddClubAsync(string adminId, ClubAddRequest club)
        {
            club.ImageUrl = await GetImageUrl(file: club.ImageFile);
            Club newClub = new Club()
            {
                Name = club.Name,
                Description = club.Description,
                ImageUrl = club.ImageUrl ,
                Hidden = club.Hidden ?? false,
                ApplicationUserId = adminId,
                //ApplicationUser = await _userManager.FindByIdAsync(adminId),

            };
            var user = _unitOfWork.Club.AddClub(adminId, newClub);
            _unitOfWork.save();
            return newClub;
        }
        public async Task<List<Club>> GetAllClubsAsync()
        {
            return await _unitOfWork.Club.GetAll();
        }

        public async Task<List<Club>> GetAdminClubsAsync(string id)
        {
            /*var user = _userManager.FindByEmailAsync(ClaimTypes.Email);*/ // will give the user's userId

            var clubs = await _unitOfWork.Club.GetAdminClubs(id);
            return clubs;

        }
        public async Task<Club> UpdateAsync(string id, ClubAddRequest club)
        {
            club.ImageUrl = await GetImageUrl(file: club.ImageFile);
            Club c = await _unitOfWork.Club.UpdateClub(id, club);
            _unitOfWork.save();
            return c;
        }

        public async Task<bool> DeleteAsync(string id, string adminId)
        {
            var AId = _unitOfWork.Club.Get(id).Result.ApplicationUserId;
            if (AId != adminId)
            {
                return false;
            }

            if (await _unitOfWork.Club.DeleteClub(id))
            {
                _unitOfWork.saveAsync();
                return true;
            }

            return false;
        }
        public async Task<ApplicationUserClub> AddMember(string userId, Guid clubId)
        {
            var c = await _unitOfWork.Club.Get(clubId.ToString());
            if (c == null)
            {
                return null;
            }
            var a = await _unitOfWork.Club.AddMember(userId, clubId);
            _unitOfWork.save();
            return a;
        }

        public async Task<List<ApplicationUser>> GetMembers(string clubId)
        {
            var m = _unitOfWork.Club.GetMembers(clubId);
            List<ApplicationUser> members = new List<ApplicationUser>();

            foreach (ApplicationUserClub c in m)
            {
                members.Add(c.ApplicationUser);
            }


            return members;
        }

        public async Task<List<Club>> GetClubsMember(string userId)
        {
            var clubs = _unitOfWork.Club.GetClubsMember(userId);
            List<Club> c = new List<Club>();
            foreach (var club in clubs)
            {
                if (club.Club != null)
                { 
                    c.Add(club.Club);
                }
                
            }
            return c;
        }

        public async Task<bool> CheckIfMember(string userId, string clubId)
        {
            var clubs =await GetClubsMember(userId);
            foreach (var club in clubs)
            {
                if(club.Id.ToString() == clubId)
                    return true;
            }
            return false;
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
                var relativePath = Path.Combine("wwwroot", "images", "clubs");

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
            return $"/images/clubs/{filename}"; // Return the relative URL path
        }




    }
}
