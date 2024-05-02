using BookMate.Entities;
using ServiceContracts.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMate.DataAccess.IRepository
{
    public interface IApplicationUserRepository
    {
        Task<ApplicationUser> Add(ApplicationUser user);
        Task<ApplicationUser> Update(string id,ApplicationUserUpdateRequest user);
        Task<bool> Delete(ApplicationUser user);
        Task<ApplicationUser> Get(string id);
        Task<List<ApplicationUser>> GetAll();
    }
}
