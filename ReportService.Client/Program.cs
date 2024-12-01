// See https://aka.ms/new-console-template for more information

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using ReportService.Client.Hotel;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IHotelServiceClient,HotelServiceClient>();