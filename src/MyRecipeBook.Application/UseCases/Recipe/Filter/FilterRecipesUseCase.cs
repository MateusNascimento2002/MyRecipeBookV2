using Mapster;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Communication.Responses;
using MyRecipeBook.Domain.Dtos;
using MyRecipeBook.Domain.Enums.Recipe;
using MyRecipeBook.Domain.Identity;
using MyRecipeBook.Domain.Interfaces.Repositories.Recipe;

namespace MyRecipeBook.Application.UseCases.Recipe.Filter;

public class FilterRecipesUseCase : IFilterRecipesUseCase
{
    private readonly IRecipeReadOnlyRepository _recipeReadOnlyRepository;
    private readonly ILoggedUser _loggedUser;

    public FilterRecipesUseCase(IRecipeReadOnlyRepository recipeReadOnlyRepository, ILoggedUser loggedUser)
    {
        _recipeReadOnlyRepository = recipeReadOnlyRepository;
        _loggedUser = loggedUser;
    }

    public async Task<ResponseRecipesJson> Execute(RequestFilterRecipesJson? request)
    {
        var recipeFilterDto = request is null
            ? new RecipeFilterDto()
            : new RecipeFilterDto
            {
                CookTime = (CookTime?)request.CookTime,
                SearchTerm = request.SearchTerm,
                DishTypes = request.DishTypes.Select(dishType => (DishType)dishType).ToList()
            };

        var recipes = await _recipeReadOnlyRepository.FilterRecipes(_loggedUser.GetUserId(), recipeFilterDto);
        var response = new ResponseRecipesJson()
        {
            Recipes = recipes.Adapt<IList<ResponseRecipeSummaryJson>>()
        };

        return response;
    }
}