using MyRecipeBook.Domain.Security.Tokens;

namespace MyRecipeBook.API.Token;

internal sealed class HttpContextTokenProvider(IHttpContextAccessor httpContextAccessor) : IAccessTokenProvider
{
    public string GetToken()
    {
        var accessToken = httpContextAccessor.HttpContext!.Request.Headers.Authorization.ToString();
        return accessToken["Bearer ".Length..];
    }
}