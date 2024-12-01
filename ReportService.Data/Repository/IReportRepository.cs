using ReportService.Data.Entity;
using ReportService.Data.Enum;

namespace ReportService.Data.Repository
{
    public interface IReportRepository
    {
     
        Task<List<ReportEntity>> GetAllReportsAsync();
        Task<ReportEntity?> GetReportByIdAsync(Guid id);
        Task AddReportAsync(ReportEntity report);
        Task DeleteReportAsync(Guid id);
        Task<List<ReportEntity>> GetReportsByStatusAsync(ReportStatus status);

        Task<List<ReportDetailEntity>> GetAllDetailsAsync();
        Task<ReportDetailEntity?> GetDetailByIdAsync(Guid id);
        Task<List<ReportDetailEntity>> GetDetailsByReportIdAsync(Guid reportId);
        Task AddDetailAsync(ReportDetailEntity detail);
        Task DeleteDetailAsync(Guid id);
    }
}
