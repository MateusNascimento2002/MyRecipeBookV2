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

namespace WebApi.Tests.Recipe.Update;

public class UpdateRecipeByIdTests : BaseIntegrationTest
{
    private const string REQUEST_URI = "/recipes";
    private readonly UserIdentityManager _user1;
    private readonly string _tokenUserNotFoundInDatabase;

    public UpdateRecipeByIdTests(MyRecipeBookApplicationFactory factory) : base(factory)
    {
        _user1 = factory.User1;
        _tokenUserNotFoundInDatabase = factory.TOKEN_USER_NOT_FOUND_IN_DATABASE;
    }

    [Fact]
    public async Task Success()
    {
        var recipe = _user1.GetRecipe();

        var request = RequestRecipeJsonBuilder.Build();

        var result = await Put($"{REQUEST_URI}/{recipe.Id}", request, _user1.GetAccessToken());

        result.StatusCode.ShouldBe(HttpStatusCode.NoContent);

        var updatedRecipe = await DbContext.Recipes
            .AsNoTracking()
            .Include(r => r.Ingredients)
            .Include(r => r.Instructions)
            .Include(r => r.DishTypes)
            .FirstOrDefaultAsync(r => r.Id == recipe.Id && r.UserId == _user1.GetId());

        updatedRecipe.ShouldNotBeNull();
        updatedRecipe.ShouldSatisfyAllConditions(recipeOnDatabase =>
        {
            recipeOnDatabase.Title.ShouldBe(request.Title);
            recipeOnDatabase.Description.ShouldBe(request.Description);
            recipeOnDatabase.CookTime.ToString().ShouldBe(request.CookTime.ToString());
            recipeOnDatabase.IsActive.ShouldBeTrue();
            recipeOnDatabase.Ingredients.Select(ingredient => ingredient.Item).ShouldBe(request.Ingredients, true);
            recipeOnDatabase.Instructions.Select(instruction => instruction.Description)
                .ShouldBe(request.Instructions.Select(instruction => instruction.Description), true);
            recipeOnDatabase.DishTypes.Select(dishType => dishType.Type.ToString())
                .ShouldBe(request.DishTypes.Select(dishType => dishType.ToString()), true);
        });
    }

    [Theory]
    [ClassData(typeof(CultureInlineData))]
    public async Task Validate_ShouldBeAnErrorResponse_WhenTitleIsEmpty(string culture)
    {
        var recipe = _user1.GetRecipe();

        var request = RequestRecipeJsonBuilder.Build();
        request.Title = string.Empty;

        var result = await Put($"{REQUEST_URI}/{recipe.Id}", request, _user1.GetAccessToken(), culture);

        result.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

        await using var responseBody = await result.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();

        var expectedMessage = ResourceMessagesException.ResourceManager.GetString(
            nameof(ResourceMessagesException.VALIDATION_RECIPE_TITLE_REQUIRED),
            CultureInfo.GetCultureInfo(culture));

        errors.ShouldSatisfyAllConditions(errorsList =>
        {
            errorsList.Count().ShouldBe(1);
            errorsList.ShouldContain(error =>
                error.GetString().IsNotEmpty() && error.GetString()!.Equals(expectedMessage));
        });

        var recipeWasUpdated = await DbContext.Recipes
            .AsNoTracking()
            .AnyAsync(r => r.Id == recipe.Id && r.Description.Equals(request.Description));

        recipeWasUpdated.ShouldBeFalse();
    }

    [Theory]
    [ClassData(typeof(CultureInlineData))]
    public async Task Validate_ShouldBeAnErrorResponse_WhenRecipeDoesntExist(string culture)
    {
        var request = RequestRecipeJsonBuilder.Build();

        var result = await Put($"{REQUEST_URI}/{Guid.CreateVersion7()}", request, _user1.GetAccessToken(), culture);

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
        var request = RequestRecipeJsonBuilder.Build();

        var result = await Put($"{REQUEST_URI}/{Guid.CreateVersion7()}", request, accessToken: token);

        result.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Validate_ShouldBeAnErrorResponse_WhenTokenUserDontExist()
    {
        var request = RequestRecipeJsonBuilder.Build();

        var result = await Put($"{REQUEST_URI}/{Guid.CreateVersion7()}", request,
            accessToken: _tokenUserNotFoundInDatabase);

        result.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }
}
