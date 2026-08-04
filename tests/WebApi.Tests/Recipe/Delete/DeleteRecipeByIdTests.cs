using System.Globalization;
using System.Net;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using MyRecipeBook.Domain.Extensions;
using MyRecipeBook.Exception;
using Shouldly;
using WebApi.Tests.InlineData;
using WebApi.Tests.Resources;

namespace WebApi.Tests.Recipe.Delete;

public class DeleteRecipeByIdTests : BaseIntegrationTest
{
    private const string REQUEST_URI = "/recipes";
    private readonly UserIdentityManager _user1;
    private readonly string _tokenUserNotFoundInDatabase;

    public DeleteRecipeByIdTests(MyRecipeBookApplicationFactory factory) : base(factory)
    {
        _user1 = factory.User1;
        _tokenUserNotFoundInDatabase = factory.TOKEN_USER_NOT_FOUND_IN_DATABASE;
    }

    [Fact]
    public async Task Success()
    {
        var recipe = _user1.GetRecipe();

        var result = await Delete($"{REQUEST_URI}/{recipe.Id}", accessToken: _user1.GetAccessToken());

        result.StatusCode.ShouldBe(HttpStatusCode.NoContent);

        var recipeExist = await DbContext.Recipes.AnyAsync(r => r.Id == recipe.Id && r.UserId == _user1.GetId());

        recipeExist.ShouldBeFalse();
    }

    [Theory]
    [ClassData(typeof(CultureInlineData))]
    public async Task Validate_ShouldBeAnErrorResponse_WhenRecipeDoesntExist(string culture)
    {
        var result = await Delete($"{REQUEST_URI}/{Guid.CreateVersion7()}", accessToken: _user1.GetAccessToken(),
            culture: culture);

        result.StatusCode.ShouldBe(HttpStatusCode.NotFound);

        await using var responseBody = await result.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();

        var expectedMessage = ResourceMessagesException.ResourceManager.GetString(
            nameof(ResourceMessagesException.VALIDATION_RECIPE_NOT_FOUND),
            CultureInfo.GetCultureInfo(culture));

        errors.ShouldSatisfyAllConditions(errorsList =>
        {
            errorsList.Count().ShouldBe(1);
            errorsList.ShouldContain(error =>
                error.GetString().IsNotEmpty() && error.GetString()!.Equals(expectedMessage));
        });
    }

    [Theory]
    [InlineData("invalid")]
    [InlineData(" ")]
    public async Task Validate_ShouldBeAnErrorResponse_WhenTokenIsInvalid(string token)
    {
        var result = await Get($"{REQUEST_URI}/{Guid.CreateVersion7()}", accessToken: token);

        result.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Validate_ShouldBeAnErrorResponse_WhenTokenUserDontExist()
    {
        var result = await Get($"{REQUEST_URI}/{Guid.CreateVersion7()}", accessToken: _tokenUserNotFoundInDatabase);

        result.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }
}