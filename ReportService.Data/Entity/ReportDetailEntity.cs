

using App.Core.BaseClass;

namespace ReportService.Data.Entity
{
    public class ReportDetailEntity :BaseEntity
    {
        public Guid Id { get; set; } 
        public string Location { get; set; }  = string.Empty;
        public int HotelCount { get; set; } 
        public int PhoneNumberCount { get; set; } 
        public Guid ReportId { get; set; } 
        public required ReportEntity Report { get; set; }
    }
}
