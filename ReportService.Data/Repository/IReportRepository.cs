using ReportService.Data.Entity;
using ReportService.Data.Enum;

namespace ReportService.Data.Repository
{
    public interface IReportRepository
    {
     
        Task<List<ReportEntity>> GetAllReportsAsync(CancellationToken cancellationToken = default);
        Task<ReportEntity?> GetReportByIdAsync(Guid id);
        Task<Guid> AddReportAsync(ReportEntity report);
        Task UpdateReportAsync (ReportEntity report);
        Task DeleteReportAsync(Guid id);
        Task<List<ReportEntity>> GetReportsByStatusAsync(ReportStatus status);
    }
}
