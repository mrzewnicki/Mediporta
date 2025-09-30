using Mediporta.Api.Extensions;
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


builder.Services.AddCors(options =>
{
    options.AddPolicy("DefaultPolicy",
        policy => policy
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod());
});

builder.Services.AddControllers();
builder.Services.AddOpenApi();

// Connection string from appsettings.json
var connString = configuration.GetConnectionString("Default") ?? "Data Source=Mediporta.db";
builder.Services.AddDbContext<MediportaDbContext>(options => options.UseSqlite(connString));

builder.Services.AddScoped<ITagsRepository, TagsRepository>();

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

app.Services.PrepareDatabase();
await app.Services.DownloadBaseDataAsync();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseCors("DefaultPolicy");
app.MapControllers();

app.Run();