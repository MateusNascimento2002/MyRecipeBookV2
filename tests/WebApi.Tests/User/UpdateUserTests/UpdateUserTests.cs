using System.Globalization;
using System.Net;
using System.Text.Json;
using CommonTestUtilities.Requests;
using Microsoft.EntityFrameworkCore;
using MyRecipeBook.Domain.Extensions;
using MyRecipeBook.Exception;
using Shouldly;
using WebApi.Tests.InlineData;
using WebApi.Tests.Resources;

namespace WebApi.Tests.User.UpdateUserTests;

public class UpadateUsersTests : BaseIntegrationTest
{
    private const string REQUEST_URI = "/users";
    private readonly string _tokenUserNotFoundInDatabase;
    private readonly UserIdentityManager _user1;

    public UpadateUsersTests(MyRecipeBookApplicationFactory factory) : base(factory)
    {
        _user1 = factory.User1;
        _tokenUserNotFoundInDatabase = factory.TOKEN_USER_NOT_FOUND_IN_DATABASE;
    }

    [Fact]
    public async Task Success()
    {
        var request = RequestUpdateUserJsonBuilder.Build();

        var result = await Put(REQUEST_URI, request, _user1.GetAccessToken());

        result.StatusCode.ShouldBe(HttpStatusCode.NoContent);

        var userExistis = await DbContext.Users.AnyAsync(user =>
            user.IsActive && user.Id == _user1.GetId() && user.Name.Equals(request.Name) &&
            user.Email.Equals(request.Email));

        userExistis.ShouldBeTrue();
    }

    [Theory]
    [InlineData("invalid")]
    [InlineData(" ")]
    public async Task Validate_ShouldBeAnErrorResponse_WhenTokenIsInvalid(string token)
    {
        var request = RequestUpdateUserJsonBuilder.Build();

        var result = await Put(REQUEST_URI, request, token);

        result.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Validate_ShouldBeAnErrorResponse_WhenTokenUserDontExist()
    {
        var request = RequestUpdateUserJsonBuilder.Build();

        var result = await Put(REQUEST_URI, request, _tokenUserNotFoundInDatabase);

        result.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }

    [Theory]
    [ClassData(typeof(CultureInlineData))]
    public async Task Validate_ShouldBeAnErrorResponse_WhenNameIsEmpty(string culture)
    {
        var request = RequestUpdateUserJsonBuilder.Build();
        request.Name = string.Empty;

        var result = await Put(REQUEST_URI, request, _user1.GetAccessToken(), culture);

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
            errorsList.ShouldContain(error =>
                error.GetString().IsNotEmpty() && error.GetString()!.Equals(expectedMessage));
        });
    }
}