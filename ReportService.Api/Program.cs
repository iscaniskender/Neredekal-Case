using ReportService.Api.Middleware;
using ReportService.Application;
using ReportService.Client;
using ReportService.Consumer;
using ReportService.Data;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.ConfigureMessageConsumer(builder.Configuration)
    .ConfigureApplication(builder.Configuration)
    .ConfigureData(builder.Configuration)
    .ConfigureClient(builder.Configuration);


Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .WriteTo.Elasticsearch(new Serilog.Sinks.Elasticsearch.ElasticsearchSinkOptions(new Uri("http://elasticsearch:9200"))
    {
        AutoRegisterTemplate = true,
        IndexFormat = "hotelservice-logs-{0:yyyy.MM.dd}"
    })
    .Enrich.FromLogContext()
    .CreateLogger();

var app = builder.Build();

app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

app.Services.ApplyMigrations();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
