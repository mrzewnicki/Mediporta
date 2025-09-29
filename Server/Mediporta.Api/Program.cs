using MediatR;
using Mediporta.Core.CQRS.Queries;
using Mediporta.Data;
using Mediporta.Data.Repositories;
using Mediporta.StackOverflow.ApiClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

// Configure Kestrel endpoints/ports from configuration
builder.WebHost.ConfigureKestrel(options =>
{
    configuration.GetSection("Kestrel").Bind(options);
});

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// QueryOptions accessor (populated by middleware)
builder.Services.AddScoped<Mediporta.Api.Middleware.IQueryOptionsAccessor, Mediporta.Api.Middleware.QueryOptionsAccessor>();

// Connection string from appsettings.json
var connString = configuration.GetConnectionString("Default") ?? "Data Source=Mediporta.db";
builder.Services.AddDbContext<MediportaDbContext>(options => options.UseSqlite(connString));

// Repositories
builder.Services.AddScoped<ITagsRepository, TagsRepository>();

// Options binding for StackOverflow API client configuration
builder.Services
    .AddOptions<StackOverflowApiClientConfiguration>()
    .Bind(configuration.GetSection("StackOverflowApi"))
    .ValidateDataAnnotations()
    .ValidateOnStart();

// Register HttpClient factory and StackOverflowApiClient service
builder.Services.AddHttpClient();
builder.Services.AddTransient<IStackOverflowApiClient, StackOverflowApiClient>(sp =>
{
    var factory = sp.GetRequiredService<IHttpClientFactory>();
    var httpClient = factory.CreateClient();
    var cfg = sp.GetRequiredService<IOptions<StackOverflowApiClientConfiguration>>().Value;
    return new StackOverflowApiClient(httpClient, cfg);
});

// MediatR - scan Mediporta.Core for handlers
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<GetTagsQuery.Request>());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// Populate query options from query string
app.UseMiddleware<Mediporta.Api.Middleware.QueryOptionsMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();