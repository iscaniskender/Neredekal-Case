using Microsoft.EntityFrameworkCore;
using ReportService.Application.Mapping;
using ReportService.Application.MediatR;
using ReportService.Data.Context;
using ReportService.Data.Repository;
using MassTransit;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();


builder.Services.RegisterRequestHandlers();

builder.Services.AddAutoMapper(typeof(ReportMapping));

//Burada duramaz ayrı bir yerde tanımlanmalı
builder.Services.AddScoped<IReportRepository, ReportRepository>();

builder.Services.AddDbContext<ReportDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("ReportDatabase")));

//.jsondan okunmalı
builder.Services.AddHttpClient("HotelServiceClient", client =>
{
    client.BaseAddress = new Uri("http://localhost:5076");
});

//buradan kalkmalı
builder.Services.AddMassTransit(x =>
{
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("rabbitmq", "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });
        
        cfg.UseMessageRetry(r => r.Interval(3, TimeSpan.FromSeconds(5)));
    });
});

var app = builder.Build();


using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ReportDbContext>();
    dbContext.Database.Migrate();
}
// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
