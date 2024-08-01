using BookMate.Entities;
using ServiceContracts.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMate.DataAccess.IRepository
{
    public interface IReportRepository
    {
        Task<Report> Add(Report report);
        Task<Report> Update(Guid id, Report report);
        Task<bool> Delete(Report report);
        Task<Report> Get(Guid id);
        Task<List<Report>> GetAll();
    }
}
