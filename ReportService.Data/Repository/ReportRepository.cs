using global::ReportService.Data.Context;
using global::ReportService.Data.Entity;
using global::ReportService.Data.Enum;
using Microsoft.EntityFrameworkCore;

namespace ReportService.Data.Repository
{

    public class ReportRepository(ReportDbContext context) : IReportRepository
    {
        public async Task<List<ReportEntity>> GetAllReportsAsync( CancellationToken cancellationToken = default)
        {
            return await context.Reports
                .Where(x=>x.IsActive)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }

        public async Task<ReportEntity?> GetReportByIdAsync(Guid id)
        {
            return await context.Reports
                .FirstOrDefaultAsync(r => r.Id == id && r.IsActive);
        }

        public async Task<Guid> AddReportAsync(ReportEntity report)
        {
            await context.Reports.AddAsync(report);
            await context.SaveChangesAsync();

            return report.Id;
        }

        public async Task UpdateReportAsync(ReportEntity report)
        {
            context.Reports.Update(report);
            await context.SaveChangesAsync();
        }

        public async Task DeleteReportAsync(Guid id)
        {
            var report = await context.Reports.FindAsync(id);
            if (report != null)
            {
                context.Reports.Remove(report);
                await context.SaveChangesAsync();
            }
        }
        public async Task<List<ReportEntity>> GetReportsByStatusAsync(ReportStatus status)
        {
            return await context.Reports
                .Where(r => r.Status == status)
                .ToListAsync();
        }
    }
}
