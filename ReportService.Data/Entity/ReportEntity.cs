using App.Core.BaseClass;
using ReportService.Data.Enum;

namespace ReportService.Data.Entity
{
    public class ReportEntity :BaseEntity
    {
        public Guid Id { get; set; }
        public ReportStatus Status { get; set; } = ReportStatus.Preparing;
        public string Location { get; set; }  = string.Empty;
        public int HotelCount { get; set; } = 0;
        public int PhoneNumberCount { get; set; } = 0;
    }
}
