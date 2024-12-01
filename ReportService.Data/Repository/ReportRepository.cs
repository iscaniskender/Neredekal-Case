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
                .Where(x=>x.IsActive)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<ReportEntity?> GetReportByIdAsync(Guid id)
        {
            return await _context.Reports
                .FirstOrDefaultAsync(r => r.Id == id && r.IsActive);
        }

        public async Task<Guid> AddReportAsync(ReportEntity report)
        {
            await _context.Reports.AddAsync(report);
            await _context.SaveChangesAsync();

            return report.Id;
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
    }
}
