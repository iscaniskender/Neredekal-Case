namespace App.Core.Shared.Messages;

public class ReportCreatedEvent
{
    public Guid ReportId { get; set; }
    public string? Location { get; set; } 
}