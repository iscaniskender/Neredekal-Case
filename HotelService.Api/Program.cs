using HotelService.Application;
using HotelService.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.ConfigureApplication(builder.Configuration)
    .ConfigureData(builder.Configuration);

var app = builder.Build();

app.Services.ApplyMigrations();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
