using Mapster;
using MyRecipeBook.Communication.Responses;
using MyRecipeBook.Domain.Identity;
using MyRecipeBook.Domain.Interfaces.Repositories.Recipe;

namespace MyRecipeBook.Application.UseCases.Recipe.GetRecent;

public class GetRecentRecipesUseCase : IGetRecentRecipesUseCase
{
    private readonly IRecipeReadOnlyRepository _recipeReadOnlyRepository;
    private readonly ILoggedUser _loggedUser;

    public GetRecentRecipesUseCase(IRecipeReadOnlyRepository recipeReadOnlyRepository, ILoggedUser loggedUser)
    {
        _recipeReadOnlyRepository = recipeReadOnlyRepository;
        _loggedUser = loggedUser;
    }
    
    public async Task<ResponseRecipesJson> Execute()
    {
        var recipes = await _recipeReadOnlyRepository.GetRecentRecipes(_loggedUser.GetUserId());
        var response = new ResponseRecipesJson()
        {
            Recipes = recipes.Adapt<IList<ResponseRecipeSummaryJson>>()
        };

        return response;
    }
}