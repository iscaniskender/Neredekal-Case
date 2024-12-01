using global::ReportService.Data.Context;
using global::ReportService.Data.Entity;
using global::ReportService.Data.Enum;
using Microsoft.EntityFrameworkCore;

namespace ReportService.Data.Repository
{

    public class ReportRepository : IReportRepository
    {
        private readonly ReportDbContext _context;

        public ReportRepository(ReportDbContext context)
        {
            _context = context;
        }

        public async Task<List<ReportEntity>> GetAllReportsAsync()
        {
            return await _context.Reports
                .Include(r => r.Details)
                .ToListAsync();
        }

        public async Task<ReportEntity?> GetReportByIdAsync(Guid id)
        {
            return await _context.Reports
                .Include(r => r.Details)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task AddReportAsync(ReportEntity report)
        {
            await _context.Reports.AddAsync(report);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateReportAsync(ReportEntity report)
        {
            _context.Reports.Update(report);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteReportAsync(Guid id)
        {
            var report = await _context.Reports.FindAsync(id);
            if (report != null)
            {
                _context.Reports.Remove(report);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<List<ReportEntity>> GetReportsByStatusAsync(ReportStatus status)
        {
            return await _context.Reports
                .Where(r => r.Status == status)
                .ToListAsync();
        }

        public async Task<List<ReportDetailEntity>> GetAllDetailsAsync()
        {
            return await _context.ReportDetails.ToListAsync();
        }

        public async Task<ReportDetailEntity?> GetDetailByIdAsync(Guid id)
        {
            return await _context.ReportDetails.FindAsync(id);
        }

        public async Task<List<ReportDetailEntity>> GetDetailsByReportIdAsync(Guid reportId)
        {
            return await _context.ReportDetails
                .Where(d => d.ReportId == reportId)
                .ToListAsync();
        }

        public async Task AddDetailAsync(ReportDetailEntity detail)
        {
            await _context.ReportDetails.AddAsync(detail);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateDetailAsync(ReportDetailEntity detail)
        {
            _context.ReportDetails.Update(detail);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteDetailAsync(Guid id)
        {
            var detail = await _context.ReportDetails.FindAsync(id);
            if (detail != null)
            {
                _context.ReportDetails.Remove(detail);
                await _context.SaveChangesAsync();
            }
        }
    }
}
