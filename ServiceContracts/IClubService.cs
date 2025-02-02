﻿using BookMate.Entities;

using ServiceContracts.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts
{
    public interface IClubService
    {
        Task<Club> GetClub(string id);
        Task<bool> DeleteAsync(string id,string adminId);
        Task<Club> UpdateAsync(string id, ClubAddRequest club);
        Task<List<Club>> GetAdminClubsAsync(string id);
        Task<List<Club>> GetAllClubsAsync();
        Task<Club> AddClubAsync(string adminId, ClubAddRequest club);
        Task<ApplicationUserClub> AddMember(string userId, Guid clubId);
        Task<List<ApplicationUser>> GetMembers(string clubId);
        Task<List<Club>> GetClubsMember(string userId);
        Task<bool> CheckIfMember(string userId, string clubId);


    }
}
