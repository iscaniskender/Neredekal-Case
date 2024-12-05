using App.Core.Shared.Messages;
using MassTransit;
using ReportService.Client.Hotel;
using ReportService.Consumer.Enum;
using ReportService.Data.Entity;
using ReportService.Data.Enum;
using ReportService.Data.Repository;

namespace ReportService.Consumer.ReportConsumer;

public class ReportCreatedConsumer(IHotelServiceClient hotelService, IReportRepository reportRepository)
    : IConsumer<ReportCreatedEvent>
{
    public async Task Consume(ConsumeContext<ReportCreatedEvent> context)
    {
        try
        {
            var reportEvent = context.Message;
            
            var result = await hotelService.GetHotelByLocationAsync(reportEvent.Location);
            var hotels = result?.Data ?? [];
            
            await Task.Delay(10000);
            
            var reportEntity = new ReportEntity
            {
                Id = reportEvent.ReportId,
                Status = ReportStatus.Completed,
                Location = reportEvent.Location,
                HotelCount = hotels.Length,
                PhoneNumberCount = hotels
                    .SelectMany(hotel => hotel.ContactInfos)
                    .Count(contact => contact.Type == ContactType.PhoneNumber.ToString())
            };
        
            await reportRepository.UpdateReportAsync(reportEntity);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error processing message: {ex.Message}");
        }
    }
}