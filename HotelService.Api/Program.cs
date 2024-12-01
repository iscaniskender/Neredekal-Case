using App.Core.Results;
using HotelService.Application.Hotel.Query;
using HotelService.Application.Mapping;
using HotelService.Application.MediatR;
using HotelService.Data.Context;
using HotelService.Data.Repository;
using HotelService.Data.Repository.AuthorizedPerson;
using HotelService.Data.Repository.ContactInfo;
using HotelService.Data.Repository.Hotel;
using MediatR;
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

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
