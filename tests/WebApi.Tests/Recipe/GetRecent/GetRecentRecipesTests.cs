using System.Net;
using System.Text.Json;
using Shouldly;
using WebApi.Tests.Resources;

namespace WebApi.Tests.Recipe.GetRecent;

public class GetRecentRecipesTests : BaseIntegrationTest
{
    private const string REQUEST_URI = "/recipes/recent";
    private readonly UserIdentityManager _user1;
    private readonly UserIdentityManager _userWithoutRecipes;
    private readonly string _tokenUserNotFoundInDatabase;

    public GetRecentRecipesTests(MyRecipeBookApplicationFactory factory) : base(factory)
    {
        _user1 = factory.User1;
        _userWithoutRecipes = factory.UserWithoutRecipes;
        _tokenUserNotFoundInDatabase = factory.TOKEN_USER_NOT_FOUND_IN_DATABASE;
    }

    [Fact]
    public async Task Success()
    {
        var recipe = _user1.GetRecipe();

        var result = await Get(REQUEST_URI, accessToken: _user1.GetAccessToken());

        result.StatusCode.ShouldBe(HttpStatusCode.OK);

        await using var responseBody = await result.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        var recipes = responseData.RootElement.GetProperty("recipes").EnumerateArray().ToList();

        recipes.ShouldSatisfyAllConditions(recipesList =>
        {
            recipesList.ShouldHaveSingleItem();
            recipesList.ShouldContain(recentRecipe => recentRecipe.GetProperty("id").GetGuid() == recipe.Id);
            recipesList.ShouldContain(recentRecipe =>
                recentRecipe.GetProperty("title").GetString()!.Equals(recipe.Title));
        });
    }

    [Fact]
    public async Task Success_ShouldReturnEmptyList_WhenUserHasNoRecipes()
    {
        var result = await Get(REQUEST_URI, accessToken: _userWithoutRecipes.GetAccessToken());

        result.StatusCode.ShouldBe(HttpStatusCode.OK);

        await using var responseBody = await result.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        responseData.RootElement.GetProperty("recipes").EnumerateArray().ShouldBeEmpty();
    }

    [Theory]
    [InlineData("invalid")]
    [InlineData(" ")]
    public async Task Validate_ShouldBeAnErrorResponse_WhenTokenIsInvalid(string token)
    {
        var result = await Get(REQUEST_URI, accessToken: token);

        result.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Validate_ShouldBeAnErrorResponse_WhenTokenUserDontExist()
    {
        var result = await Get(REQUEST_URI, accessToken: _tokenUserNotFoundInDatabase);

        result.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }
}