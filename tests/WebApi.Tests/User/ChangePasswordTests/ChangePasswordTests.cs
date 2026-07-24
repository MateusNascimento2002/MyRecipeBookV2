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

namespace WebApi.Tests.User.ChangePasswordTests;

public class ChangePasswordTests : BaseIntegrationTest
{
    private const string REQUEST_URI = "/users";
    private readonly UserIdentityManager _user1;
    private readonly string _tokenUserNotFoundInDatabase;
    public ChangePasswordTests(MyRecipeBookApplicationFactory factory) : base(factory)
    {
        _user1 = factory.User1;
        _tokenUserNotFoundInDatabase = factory.TOKEN_USER_NOT_FOUND_IN_DATABASE;
    }

    [Fact]
    public async Task Success()
    {
        var request = RequestChangePasswordJsonBuilder.Build();
        request.CurrentPassword = _user1.GetPassword();

        var result = await Patch(REQUEST_URI, request, accessToken: _user1.GetAccessToken());

        result.StatusCode.ShouldBe(HttpStatusCode.NoContent);
    }
    
    [Theory]
    [InlineData("invalid")]
    [InlineData(" ")]
    public async Task Validate_ShouldBeAnErrorResponse_WhenTokenIsInvalid(string token)
    {
        var request = RequestChangePasswordJsonBuilder.Build();

        var result = await Patch(REQUEST_URI, request, accessToken: token);

        result.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }
    
    [Fact]
    public async Task Validate_ShouldBeAnErrorResponse_WhenTokenUserDontExist()
    {
        var request = RequestChangePasswordJsonBuilder.Build();

        var result = await Patch(REQUEST_URI, request, accessToken: _tokenUserNotFoundInDatabase);

        result.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }

    [Theory]
    [ClassData(typeof(CultureInlineData))]
    public async Task Validate_ShouldBeAnErrorResponse_WhenNewPasswordIsEmpty(string culture)
    {
        var request = new RequestChangePasswordJson()
        {
            CurrentPassword = _user1.GetPassword(),
            NewPassword = string.Empty
        };

        var result = await Patch(REQUEST_URI, request, accessToken: _user1.GetAccessToken(), culture: culture);

        result.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

        await using var responseBody = await result.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();

        var expectedMessage = ResourceMessagesException.ResourceManager.GetString(
            nameof(ResourceMessagesException.VALIDATION_PASSWORD_REQUIRED),
            CultureInfo.GetCultureInfo(culture));

        errors.ShouldSatisfyAllConditions(errorsList =>
        {
            errorsList.Count().ShouldBe(1);
            errorsList.ShouldContain(error =>
                error.GetString().IsNotEmpty() && error.GetString()!.Equals(expectedMessage));
        });
    }
}

