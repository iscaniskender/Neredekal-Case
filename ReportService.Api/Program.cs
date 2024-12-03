using ReportService.Application;
using ReportService.Client;
using ReportService.Consumer;
using ReportService.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.ConfigureMessageConsumer(builder.Configuration)
    .ConfigureApplication(builder.Configuration)
    .ConfigureData(builder.Configuration)
    .ConfigureClient(builder.Configuration);

var app = builder.Build();

app.Services.ApplyMigrations();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
