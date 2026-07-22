using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.Extensions.DependencyInjection;
using MyRecipeBook.Domain.Extensions;
using MyRecipeBook.Infrastructure.DataAccess;

namespace WebApi.Tests;

public class BaseIntegrationTest : IClassFixture<MyRecipeBookApplicationFactory>, IDisposable
{
    private readonly HttpClient _httpClient;
    private readonly IServiceScope _scope;
    internal readonly MyRecipeBookDbContext DbContext;

    public BaseIntegrationTest(MyRecipeBookApplicationFactory factory)
    {
        _httpClient = factory.CreateClient();

        _scope = factory.Services.CreateScope();

        DbContext = _scope.ServiceProvider.GetRequiredService<MyRecipeBookDbContext>();
    }

    public void Dispose()
    {
        _scope?.Dispose();
        DbContext?.Dispose();
    }

    protected async Task<HttpResponseMessage> Post(string requestUri, object request, string accessToken = "", string culture = "en-US")
    {
        ChangeRequestCulture(culture);
        AuthorizeRequest(accessToken);
        return await _httpClient.PostAsJsonAsync(requestUri, request);
    }
    
    protected async Task<HttpResponseMessage> Get(string requestUri, string accessToken, string culture = "en-US")
    {
        ChangeRequestCulture(culture);
        AuthorizeRequest(accessToken);
        return await _httpClient.GetAsync(requestUri);
    }

    private void AuthorizeRequest(string accessToken)
    {
        if (accessToken.IsNotEmpty())
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
    }

    private void ChangeRequestCulture(string culture)
    {
        _httpClient.DefaultRequestHeaders.Clear();
        _httpClient.DefaultRequestHeaders.AcceptLanguage.ParseAdd(culture);
    }
}