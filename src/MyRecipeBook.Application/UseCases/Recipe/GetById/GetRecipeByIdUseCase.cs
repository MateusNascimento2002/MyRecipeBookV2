using Mapster;
using MyRecipeBook.Communication.Responses;
using MyRecipeBook.Domain.Identity;
using MyRecipeBook.Domain.Interfaces.Repositories.Recipe;
using MyRecipeBook.Exception;
using MyRecipeBook.Exception.ExceptionBase;

namespace MyRecipeBook.Application.UseCases.Recipe.GetById;

public class GetRecipeByIdUseCase : IGetRecipeByIdUseCase
{
    private readonly IRecipeReadOnlyRepository _recipeReadOnlyRepository;
    private readonly ILoggedUser _loggedUser;

    public GetRecipeByIdUseCase(IRecipeReadOnlyRepository recipeReadOnlyRepository, ILoggedUser loggedUser)
    {
        _recipeReadOnlyRepository = recipeReadOnlyRepository;
        _loggedUser = loggedUser;
    }
    
    public async Task<ResponseRecipeJson> Execute(Guid id)
    {
        var recipe = await _recipeReadOnlyRepository.GetById(id, _loggedUser.GetUserId());

        if (recipe is null)
            throw new NotFoundException(ResourceMessagesException.VALIDATION_RECIPE_NOT_FOUND);
        
        return recipe.Adapt<ResponseRecipeJson>();
    }
}