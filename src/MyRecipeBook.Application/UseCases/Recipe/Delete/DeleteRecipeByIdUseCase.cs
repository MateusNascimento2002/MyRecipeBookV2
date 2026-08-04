using MyRecipeBook.Domain.Identity;
using MyRecipeBook.Domain.Interfaces.Repositories.Recipe;
using MyRecipeBook.Exception;
using MyRecipeBook.Exception.ExceptionBase;

namespace MyRecipeBook.Application.UseCases.Recipe.Delete;

public class DeleteRecipeByIdUseCase : IDeleteRecipeByIdUseCase
{
    private readonly ILoggedUser _loggedUser;
    private readonly IRecipeWriteOnlyRepository _recipeWriteOnlyRepository;

    public DeleteRecipeByIdUseCase(IRecipeWriteOnlyRepository recipeWriteOnlyRepository, ILoggedUser loggedUser)
    {
        _recipeWriteOnlyRepository = recipeWriteOnlyRepository;
        _loggedUser = loggedUser;
    }
    
    public async Task Execute(Guid id)
    {
        var deleted = await _recipeWriteOnlyRepository.DeleteById(id, _loggedUser.GetUserId());
        if (deleted == false)
            throw new NotFoundException(ResourceMessagesException.VALIDATION_RECIPE_NOT_FOUND);
    }
}