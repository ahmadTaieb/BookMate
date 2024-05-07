using BookMate.Entities;
using ServiceContracts.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMate.DataAccess.IRepository
{
    public interface IClubRepository
    {
        Task<Club> Get(string id);
        Task<List<Club>> GetAll();
        Task<Club> AddClub(Club club);
        Task<Club> UpdateClub(string id,ClubAddRequest club);
        Task<bool> DeleteClub(string id);
        Task<List<Club>> GetAdminClubs(string adminId);
        Task<ApplicationUser> GetAdminClub(string clubId);
    }
}
