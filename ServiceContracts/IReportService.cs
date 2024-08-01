using BookMate.Entities;
using ServiceContracts.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts
{
    public interface IReportService
    {
        Task<Report> CreateAsync(Report report);
        Task<Report> UpdateAsync(Guid id, Report report);
        Task<Report> DeleteAsync(Guid id);
        Task<Report> GetAsync(Guid id);
        Task<List<Report>> GetAllAsync();
    }
}
