using BookMate.DataAccess.Data;
using BookMate.DataAccess.IRepository;
using BookMate.Entities;
using Microsoft.EntityFrameworkCore;
using ServiceContracts.DTO;

namespace BookMate.DataAccess.Repository
{
    public class ClubRepsitory : IClubRepository
    {
        private ApplicationDbContext _db;

        public ClubRepsitory(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<Club> AddClub(Club club)
        {
            _db.Clubs.Add(club);
            return club;
        }

        public async Task<bool> DeleteClub(string id)
        {
            var club = await _db.Clubs.FirstOrDefaultAsync(i => i.Id.ToString() == id);
            _db?.Clubs.Remove(club);
            return true;
        }

        public async Task<Club> Get(string id)
        {
            var club = await _db.Clubs.Include(i => i.ApplicationUser).FirstOrDefaultAsync(i => i.Id.ToString() == id);
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
            var admin =  _db.ApplicationUsers.FirstOrDefaultAsync(i => i.Id.ToString() == adminId);
            var clubs =  _db?.Clubs.Where(i => i.ApplicationUserId == adminId).ToList();
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
            matchingClub.ApplicationUserId = club.ApplicationUserId ?? club.ApplicationUserId;
            matchingClub.ImageUrl = club.ImageUrl ?? matchingClub.ImageUrl;
            matchingClub.Hidden = club.Hidden ?? matchingClub.Hidden;
            if (club.ApplicationUser != null)
            {
                matchingClub.ApplicationUser = club.ApplicationUser;
            }
            else
            {
                matchingClub.ApplicationUser = _db.ApplicationUsers.FirstOrDefault(i => i.Id.ToString() == matchingClub.ApplicationUserId);
            }

            return matchingClub;

        }
    }
}
