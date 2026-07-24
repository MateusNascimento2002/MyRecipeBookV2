using System.Globalization;
using System.Net;
using System.Text.Json;
using CommonTestUtilities.Requests;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Domain.Extensions;
using MyRecipeBook.Exception;
using Shouldly;
using WebApi.Tests.InlineData;
using WebApi.Tests.Resources;

namespace WebApi.Tests.Login.WithEmailAndPassword;

public class LoginWithEmailAndPasswordTests : BaseIntegrationTest
{
    private const string REQUEST_URI = "/authentication";
    private readonly UserIdentityManager _user1;

    public LoginWithEmailAndPasswordTests(MyRecipeBookApplicationFactory factory) : base(factory)
    {
        _user1 = factory.User1;
    }

    [Fact]
    public async Task Success()
    {
        var request = new RequestLoginJson
        {
            Email = _user1.GetEmail(),
            Password = _user1.GetPassword()
        };

        var result = await Post(REQUEST_URI, request);

        result.StatusCode.ShouldBe(HttpStatusCode.OK);

        await using var responseBody = await result.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        responseData.RootElement.GetProperty("name").GetString().ShouldBe(_user1.GetName());
        //todo: correct when implement tokens.
        responseData.RootElement.GetProperty("tokens").GetProperty("accessToken").GetString().ShouldNotBeNullOrEmpty();
        responseData.RootElement.GetProperty("tokens").GetProperty("refreshToken").GetString().ShouldBeNullOrEmpty();
    }

    [Theory]
    [ClassData(typeof(CultureInlineData))]
    public async Task ShouldThrowException_WhenUserDontExist(string culture)
    {
        var request = RequestLoginJsonBuilder.Build();

        var result = await Post(REQUEST_URI, request, culture: culture);

        result.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);

        await using var responseBody = await result.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();

        var expectedMessage = ResourceMessagesException.ResourceManager.GetString(
            nameof(ResourceMessagesException.VALIDATION_LOGIN_INVALID),
            CultureInfo.GetCultureInfo(culture));

        errors.ShouldSatisfyAllConditions(errorsList =>
        {
            errorsList.Count().ShouldBe(1);
            errorsList.ShouldContain(error =>
                error.GetString().IsNotEmpty() && error.GetString()!.Equals(expectedMessage));
        });
    }

    [Theory]
    [ClassData(typeof(CultureInlineData))]
    public async Task ShouldThrowException_WhenPasswordIsWrong(string culture)
    {
        var request = RequestLoginJsonBuilder.Build();
        request.Email = _user1.GetEmail();
        var result = await Post(REQUEST_URI, request, culture: culture);

        result.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);

        await using var responseBody = await result.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();

        var expectedMessage = ResourceMessagesException.ResourceManager.GetString(
            nameof(ResourceMessagesException.VALIDATION_LOGIN_INVALID),
            CultureInfo.GetCultureInfo(culture));

        errors.ShouldSatisfyAllConditions(errorsList =>
        {
            errorsList.Count().ShouldBe(1);
            errorsList.ShouldContain(error =>
                error.GetString().IsNotEmpty() && error.GetString()!.Equals(expectedMessage));
        });
    }
}