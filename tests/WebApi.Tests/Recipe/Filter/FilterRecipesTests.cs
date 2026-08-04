using System.Net;
using System.Text.Json;
using MyRecipeBook.Communication.Requests;
using Shouldly;
using WebApi.Tests.Resources;

namespace WebApi.Tests.Recipe.Filter;

public class FilterRecipesTests : BaseIntegrationTest
{
    private const string REQUEST_URI = "/recipes/filter";
    private readonly UserIdentityManager _user1;
    private readonly UserIdentityManager _userWithoutRecipes;
    private readonly string _tokenUserNotFoundInDatabase;

    public FilterRecipesTests(MyRecipeBookApplicationFactory factory) : base(factory)
    {
        _user1 = factory.User1;
        _userWithoutRecipes = factory.UserWithoutRecipes;
        _tokenUserNotFoundInDatabase = factory.TOKEN_USER_NOT_FOUND_IN_DATABASE;
    }

    [Fact]
    public async Task Success()
    {
        var recipe = _user1.GetRecipe();

        var request = new RequestFilterRecipesJson
        {
            SearchTerm = recipe.Title,
            CookTime = (MyRecipeBook.Communication.Enums.Recipe.CookTime)recipe.CookTime,
            DishTypes = recipe.DishTypes
                .Select(dishType => (MyRecipeBook.Communication.Enums.Recipe.DishType)dishType.Type)
                .ToList()
        };

        var result = await Post(REQUEST_URI, request, _user1.GetAccessToken());

        result.StatusCode.ShouldBe(HttpStatusCode.OK);

        await using var responseBody = await result.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        var recipes = responseData.RootElement.GetProperty("recipes").EnumerateArray().ToList();

        recipes.ShouldSatisfyAllConditions(recipesList =>
        {
            recipesList.ShouldHaveSingleItem();
            recipesList.ShouldContain(filteredRecipe => filteredRecipe.GetProperty("id").GetGuid() == recipe.Id);
            recipesList.ShouldContain(filteredRecipe =>
                filteredRecipe.GetProperty("title").GetString()!.Equals(recipe.Title));
        });
    }

    [Fact]
    public async Task Success_ShouldReturnEmptyList_WhenNoRecipeMatchesTheFilter()
    {
        var request = new RequestFilterRecipesJson { SearchTerm = $"{Guid.CreateVersion7()}" };

        var result = await Post(REQUEST_URI, request, _user1.GetAccessToken());

        result.StatusCode.ShouldBe(HttpStatusCode.OK);

        await using var responseBody = await result.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        responseData.RootElement.GetProperty("recipes").EnumerateArray().ShouldBeEmpty();
    }

    [Fact]
    public async Task Success_ShouldReturnEmptyList_WhenUserHasNoRecipes()
    {
        var recipe = _user1.GetRecipe();

        var request = new RequestFilterRecipesJson { SearchTerm = recipe.Title };

        var result = await Post(REQUEST_URI, request, _userWithoutRecipes.GetAccessToken());

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
        var request = new RequestFilterRecipesJson();

        var result = await Post(REQUEST_URI, request, accessToken: token);

        result.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Validate_ShouldBeAnErrorResponse_WhenTokenUserDontExist()
    {
        var request = new RequestFilterRecipesJson();

        var result = await Post(REQUEST_URI, request, accessToken: _tokenUserNotFoundInDatabase);

        result.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }
}
