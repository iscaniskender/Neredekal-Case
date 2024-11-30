using ReportService.Data.Enum;

namespace ReportService.Data.Entity
{
    public class ReportEntity :BaseEntity
    {
        public Guid Id { get; set; } 
        public DateTime RequestedAt { get; set; }
        public ReportStatus Status { get; set; } 
        public List<ReportDetailEntity> Details { get; set; } = new();
    }
}
