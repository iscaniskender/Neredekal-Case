using MassTransit;
using Microsoft.AspNetCore.Builder;
using ReportService.Consumer;
using ReportService.Consumer.ReportConsumer;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<ReportCreatedConsumer>(); // Consumer'ı ekleyin

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("localhost", "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });

        cfg.ReceiveEndpoint("report-created-event-queue", e =>
        {
            e.ConfigureConsumer<ReportCreatedConsumer>(context);
        });
    });
});

var app = builder.Build();

app.Run();