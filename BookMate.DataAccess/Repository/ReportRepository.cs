using BookMate.DataAccess.Data;
using BookMate.DataAccess.IRepository;
using BookMate.Entities;
using Microsoft.EntityFrameworkCore;
using ServiceContracts.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMate.DataAccess.Repository
{
    public class ReportRepository : IReportRepository
    {
        private ApplicationDbContext _db;

        public ReportRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<Report> Add(Report report)
        {
            _db.Reports.Add(report);

            return report;
        }

        public async Task<bool> Delete(Report report)
        {
            _db.Reports.Remove(report);
            return true;
        }

        public async Task<Report?> Get(Guid id)
        {
            return await _db.Reports.FirstOrDefaultAsync(p => p.Id == id);

        }

        public async Task<List<Report>> GetAll()
        {
            var reports = await _db.Reports.ToListAsync();
            return reports;
        }

        public async Task<Report> Update(Guid id, Report report)
        {
            Report? matchingReport = await _db.Reports.FirstOrDefaultAsync(q => q.Id.ToString() == id.ToString());
            if (matchingReport != null)
            {
                matchingReport.PostId = report.PostId;
                matchingReport.ApplicationUserId = report.ApplicationUserId;

                _db.Reports.Update(matchingReport);
            }
            return matchingReport;
        }
    }
}
