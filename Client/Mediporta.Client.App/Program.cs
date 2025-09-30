using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Mediporta.Client.App;
using Mediporta.Client.App.Config;
using Mediporta.Client.App.Services;
using Microsoft.Extensions.Options;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var httpClientSection = builder.Configuration.GetSection(HttpClientSettings.SectionName);
builder.Services.Configure<HttpClientSettings>(options => httpClientSection.Bind(options));
builder.Services.AddHttpClient(HttpClientSettings.SectionName, (sp, client) =>
{
    var settings = sp.GetRequiredService<IOptions<HttpClientSettings>>().Value;
    var baseAddress = !string.IsNullOrEmpty(settings.BaseAddress) ? settings.BaseAddress : builder.HostEnvironment.BaseAddress;
    client.BaseAddress = new Uri(baseAddress);
});

builder.Services.AddMudServices(options =>
{
    options.PopoverOptions.ThrowOnDuplicateProvider = false;
});

builder.Services.AddTransient<ITagsService, TagsService>();

await builder.Build().RunAsync();