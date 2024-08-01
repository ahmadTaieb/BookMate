using BookMate.DataAccess.IRepository;
using BookMate.Entities;
using Microsoft.AspNetCore.Identity;
using ServiceContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class ReportService : IReportService
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private IUnitOfWork _unitOfWork;

        public ReportService(UserManager<ApplicationUser> userManager, IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
        }

        public async Task<Report> CreateAsync(Report report)
        {
            _unitOfWork.Report.Add(report);
            _unitOfWork.save();
            return  report;
        }

        public async Task<Report> DeleteAsync(Guid id)
        {
            var report = await _unitOfWork.Report.Get(id);
            await _unitOfWork.Report.Delete(report);
            _unitOfWork.save();
            return report;
        }

        public async Task<List<Report>> GetAllAsync()
        {
            return await _unitOfWork.Report.GetAll();
        }

        public Task<Report> GetAsync(Guid id)
        {
            return _unitOfWork.Report.Get(id);
        }

        public async Task<Report> UpdateAsync(Guid id, Report report)
        {
            return await _unitOfWork.Report.Update(id, report);            
        }
    }
}
