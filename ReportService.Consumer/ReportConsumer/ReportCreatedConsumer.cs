using App.Core.Shared.Messages;
using MassTransit;
using ReportService.Client.Hotel;

namespace ReportService.Consumer.ReportConsumer;

public class ReportCreatedConsumer : IConsumer<ReportCreatedEvent>
{
    private readonly IHotelServiceClient _hotelService; 
    
    public async Task Consume(ConsumeContext<ReportCreatedEvent> context)
    {
        var reportEvent = context.Message;

        Console.WriteLine($"Report {reportEvent.ReportId} is now ");

        Console.WriteLine($"Report {reportEvent.ReportId} status updated to ");
        
        await Task.CompletedTask;
    }
}