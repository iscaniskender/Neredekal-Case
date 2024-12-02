using HotelService.Application.Mapping;
using HotelService.Application.MediatR;
using HotelService.Data.Context;
using HotelService.Data.Repository;
using HotelService.Data.Repository.AuthorizedPerson;
using HotelService.Data.Repository.ContactInfo;
using HotelService.Data.Repository.Hotel;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();

builder.Services.AddDbContext<HotelDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("HotelDatabase")));

builder.Services.RegisterRequestHandlers();

builder.Services.AddAutoMapper(typeof(HotelMapping));

builder.Services.AddScoped<IHotelRepository, HotelRepository>();
builder.Services.AddScoped<IAuthorizedPersonRepository, AuthorizedPersonRepository>();
builder.Services.AddScoped<IContactInfoRepository, ContactInfoRepository>();


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<HotelDbContext>();
    dbContext.Database.Migrate();
}


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
