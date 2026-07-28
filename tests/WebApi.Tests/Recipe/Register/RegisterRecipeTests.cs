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

namespace WebApi.Tests.Recipe.Register;

public class RegisterRecipeTests : BaseIntegrationTest
{
    private const string REQUEST_URI = "/recipes";
    private readonly UserIdentityManager _user1;
    private readonly string _tokenUserNotFoundInDatabase;
    
    public RegisterRecipeTests(MyRecipeBookApplicationFactory factory) : base(factory)
    {
        _user1 = factory.User1;
        _tokenUserNotFoundInDatabase = factory.TOKEN_USER_NOT_FOUND_IN_DATABASE;
    }

    [Fact]
    public async Task Success()
    {
        var request = RequestRecipeJsonBuilder.Build();

        var result = await Post(REQUEST_URI, request, _user1.GetAccessToken());

        result.StatusCode.ShouldBe(HttpStatusCode.Created);

        await using var responseBody = await result.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        var recipeId = responseData.RootElement.GetProperty("id").GetGuid();

        recipeId.ShouldNotBe(Guid.Empty);
        responseData.RootElement.GetProperty("title").GetString().ShouldBe(request.Title);

        var registeredRecipe = await DbContext.Recipes
            .Include(recipe => recipe.Ingredients)
            .Include(recipe => recipe.Instructions)
            .Include(recipe => recipe.DishTypes)
            .FirstOrDefaultAsync(recipe => recipe.Id == recipeId);

        registeredRecipe.ShouldNotBeNull();
        registeredRecipe.ShouldSatisfyAllConditions(recipe =>
        {
            recipe.Title.ShouldBe(request.Title);
            recipe.Description.ShouldBe(request.Description);
            recipe.CookTime.ToString().ShouldBe(request.CookTime.ToString());
            recipe.UserId.ShouldBe(_user1.GetId());
            recipe.IsActive.ShouldBeTrue();
            recipe.Ingredients.Select(ingredient => ingredient.Item).ShouldBe(request.Ingredients, true);
            recipe.Instructions.Select(instruction => instruction.Description)
                .ShouldBe(request.Instructions.Select(instruction => instruction.Description), true);
            recipe.DishTypes.Select(dishType => dishType.Type.ToString())
                .ShouldBe(request.DishTypes.Select(dishType => dishType.ToString()), true);
        });
    }

    [Theory]
    [ClassData(typeof(CultureInlineData))]
    public async Task Validate_ShouldBeAnErrorResponse_WhenTitleIsEmpty(string culture)
    {
        var request = RequestRecipeJsonBuilder.Build();
        request.Title = string.Empty;

        var result = await Post(REQUEST_URI, request, _user1.GetAccessToken(), culture);

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

        var recipeExists = await DbContext.Recipes.AnyAsync(recipe =>
            recipe.Description.Equals(request.Description) && recipe.UserId == _user1.GetId());

        recipeExists.ShouldBeFalse();
    }
    
    [Theory]
    [InlineData("invalid")]
    [InlineData(" ")]
    public async Task Validate_ShouldBeAnErrorResponse_WhenTokenIsInvalid(string token)
    {
        var request = RequestRecipeJsonBuilder.Build();

        var result = await Post(REQUEST_URI, request, accessToken: token);

        result.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Validate_ShouldBeAnErrorResponse_WhenTokenUserDontExist()
    {
        var request = RequestRecipeJsonBuilder.Build();

        var result = await Post(REQUEST_URI, request, accessToken: _tokenUserNotFoundInDatabase);

        result.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }
}
