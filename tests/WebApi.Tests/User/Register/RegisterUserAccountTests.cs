using System.Globalization;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Runtime.InteropServices.JavaScript;
using System.Text.Json;
using CommonTestUtilities.Requests;
using Microsoft.AspNetCore.Mvc.Testing;
using MyRecipeBook.Domain.Extensions;
using MyRecipeBook.Exception;
using Shouldly;
using WebApi.Tests.InlineData;

namespace WebApi.Tests.User.Register;

public class RegisterUserAccountTests : IClassFixture<MyRecipeBookApplicationFactory>
{
    private readonly HttpClient _httpClient;
    private const string REQUEST_URI = "/users";
    public RegisterUserAccountTests(MyRecipeBookApplicationFactory factory)
    {
        _httpClient = factory.CreateClient();
    }
    
    [Fact]
    public async Task Success()
    {
        var request = RequestRegisterUserAccountJsonBuilder.Build();
        var result = await _httpClient.PostAsJsonAsync(REQUEST_URI, request );

        result.StatusCode.ShouldBe(HttpStatusCode.Created);

        await using var responseBody = await result.Content.ReadAsStreamAsync();
        
        var responseData = await JsonDocument.ParseAsync(responseBody);
        
        responseData.RootElement.GetProperty("name").GetString().ShouldBe(request.Name);
        //todo: correct when implement tokens.
        responseData.RootElement.GetProperty("tokens").GetProperty("accessToken").GetString().ShouldBeEmpty();
        responseData.RootElement.GetProperty("tokens").GetProperty("refreshToken").GetString().ShouldBeEmpty();
    }
    
    [Theory]
    [ClassData(typeof(CultureInlineData))]
    public async Task Validate_ShouldBeAnErrorResponse_WhenNameIsEmpty(string culture)
    {
        var request = RequestRegisterUserAccountJsonBuilder.Build();
        request.Name = string.Empty;
        _httpClient.DefaultRequestHeaders.Clear();
        _httpClient.DefaultRequestHeaders.AcceptLanguage.ParseAdd(culture);
        
        var result = await _httpClient.PostAsJsonAsync(REQUEST_URI, request );

        result.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        
        await using var responseBody = await result.Content.ReadAsStreamAsync();
        
        var responseData = await JsonDocument.ParseAsync(responseBody);

        var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();

        var expectedMessage = ResourceMessagesException.ResourceManager.GetString(
            nameof(ResourceMessagesException.VALIDATION_NAME_REQUIRED),
            CultureInfo.GetCultureInfo(culture));

        errors.ShouldSatisfyAllConditions(errorsList =>
        {
            errorsList.Count().ShouldBe(1);
            errorsList.ShouldContain(error => error.GetString().IsNotEmpty() && error.GetString()!.Equals(expectedMessage));
        });
        
    }
}