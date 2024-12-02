using App.Core.Shared.Messages;
using MassTransit;
using ReportService.Client.Hotel;
using ReportService.Client.Hotel.Model;
using ReportService.Consumer.Enum;
using ReportService.Data.Entity;
using ReportService.Data.Enum;
using ReportService.Data.Repository;

public class ReportCreatedConsumer : IConsumer<ReportCreatedEvent>
{
    private readonly IHotelServiceClient _hotelService;
    private readonly IReportRepository _reportRepository;

    public ReportCreatedConsumer(IHotelServiceClient hotelService, IReportRepository reportRepository)
    {
        _hotelService = hotelService;
        _reportRepository = reportRepository;
    }

    public async Task Consume(ConsumeContext<ReportCreatedEvent> context)
    {
        try
        {
            var reportEvent = context.Message;
            
            var result = await _hotelService.GetHotelByLocationAsync(reportEvent.Location);
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

            await _reportRepository.UpdateReportAsync(reportEntity);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error processing message: {ex.Message}");
        }
    }
}