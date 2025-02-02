﻿using BookMate.DataAccess.Data;
using BookMate.DataAccess.IRepository;
using BookMate.Entities;
using Microsoft.EntityFrameworkCore;
using ServiceContracts.DTO;

namespace BookMate.DataAccess.Repository
{
    public class ClubRepository : IClubRepository
    {
        private ApplicationDbContext _db;

        public ClubRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<Club> AddClub(string adminId, Club club)
        {
            var c = _db.Clubs.Add(club);
            
            AddMember(adminId, club.Id);
            
            return club;
        }

        public async Task<bool> DeleteClub(string id)
        {
            var posts = _db.Posts.Where(x => x.ClubId.ToString() == id).ToList();
            _db.Posts.RemoveRange(posts);
            _db.SaveChanges();

            var relation = _db.ApplicationUserClubs.Where(x => x.ClubId.ToString() == id ).ToList();
            _db.ApplicationUserClubs.RemoveRange(relation);
            _db.SaveChanges();

            var club = await _db.Clubs.FirstOrDefaultAsync(i => i.Id.ToString() == id);
            _db.Clubs.Remove(club);
            _db.SaveChanges();


            return true;
        }

        public async Task<Club> Get(string id)
        {
            Club club = await _db.Clubs.Include(i => i.ApplicationUser).FirstOrDefaultAsync(i => i.Id.ToString() == id);
            return club;
        }

        public async Task<ApplicationUser> GetAdminClub(string clubId)
        {
            var club = await _db.Clubs.FirstOrDefaultAsync(i => i.Id.ToString() == clubId);
            var admin = await _db.ApplicationUsers.FirstOrDefaultAsync(i => i.Id.ToString() == club.ApplicationUserId);
            return admin;
        }

        public async Task<List<Club>> GetAdminClubs(string adminId)
        {
            var admin =await  _db.ApplicationUsers.FirstOrDefaultAsync(i => i.Id.ToString() == adminId);
            var clubs =await  _db.Clubs.Where(i => i.ApplicationUserId == adminId).ToListAsync();
            return clubs;

        }

        public async Task<List<Club>> GetAll()
        {
            return await _db.Clubs.ToListAsync();
        }

        public async Task<Club> UpdateClub(string id, ClubAddRequest? club)
        {
            Club? matchingClub = await _db.Clubs.FirstOrDefaultAsync(i => i.Id.ToString() == id);

            if (matchingClub == null)
            {
                return null;
            }
            matchingClub.Name = club.Name ?? matchingClub.Name;
            matchingClub.Description = club.Description ?? matchingClub.Description;
            //matchingClub.ApplicationUserId = club.ApplicationUserId ?? matchingClub.ApplicationUserId;
            matchingClub.ImageUrl = club.ImageUrl ?? matchingClub.ImageUrl;
            matchingClub.Hidden = club.Hidden ?? matchingClub.Hidden;
            //if (club.ApplicationUser != null)
            //{
            //    matchingClub.ApplicationUser = club.ApplicationUser;
            //}
            //else
            //{
            //    matchingClub.ApplicationUser = _db.ApplicationUsers.FirstOrDefault(i => i.Id.ToString() == matchingClub.ApplicationUserId);
            //}
            _db.Clubs.Update(matchingClub);
            return matchingClub;

        }

        public async Task<ApplicationUserClub> AddMember(string userId, Guid clubId) 
        {
            var a = new ApplicationUserClub 
            {
                ApplicationUserId = userId,
                ClubId = clubId,
                //ApplicationUser = _db.ApplicationUsers.FirstOrDefault(i => i.Id == userId),
                //Club = _db.Clubs.FirstOrDefault(c => c.Id == clubId),
            };
            _db.ApplicationUserClubs.Add(a);
            
            return  a;
        }

        public List<ApplicationUserClub> GetMembers(string clubId)
        {
            var c =  _db.Clubs
                .Include(x => x.ApplicationUsersMember)
                .ThenInclude(y => y.ApplicationUser)
                .FirstOrDefault(i => i.Id.ToString() == clubId);
            if(c == null)
            {
                return [];
            }

            return (List<ApplicationUserClub>)c.ApplicationUsersMember;
    
        }
        public List<ApplicationUserClub> GetClubsMember(string userId)
        {
            var a = _db.ApplicationUsers.
                Include(u => u.ClubsMember)
                .ThenInclude(i => i.Club)
                .FirstOrDefault(i => i.Id == userId);

            return (List<ApplicationUserClub>)a.ClubsMember;
        }


    }
}
