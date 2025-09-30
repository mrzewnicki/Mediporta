using System.Net.Http.Json;
using Mediporta.Client.App.Config;
using Mediporta.Shared.DTOs;
using Mediporta.Shared.Helpers;
using MudBlazor;

namespace Mediporta.Client.App.Services;

public interface ITagsService
{
    Task<GetTagResultDto> GetTags(string sortExpression, int page, int pageSize);
}

public class TagsService : ITagsService
{
    private readonly HttpClient _httpClient;
    private readonly ISnackbar _snackbar;

    public TagsService(IHttpClientFactory httpFactory, ISnackbar snackbar)
    {
        _httpClient = httpFactory.CreateClient(HttpClientSettings.SectionName);
        _snackbar = snackbar;
    }

    public async Task<GetTagResultDto> GetTags(string sortExpression, int page, int pageSize)
    {
        try
        {
            var response = await _httpClient.GetFromJsonAsync<GetTagResultDto>($"Tags/GetAll?sort={sortExpression}&page={page}&pageSize={pageSize}");

            if (response is null)
                throw new Exception("Fetch data failed");

            return response;
        }
        catch (Exception e)
        {
            _snackbar.Add("Wystąpił błąd podczas pobierania danych", Severity.Error);

            if (EnvironmentHelper.IsDebug)
                Console.WriteLine(e);

            throw;
        }
    }
}