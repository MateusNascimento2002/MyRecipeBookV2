using Mapster;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Communication.Responses;
using MyRecipeBook.Domain.Identity;
using MyRecipeBook.Domain.Interfaces.Repositories.Recipe;
using MyRecipeBook.Domain.Interfaces.Repositories.UnitOfWork;
using MyRecipeBook.Exception;
using MyRecipeBook.Exception.ExceptionBase;

namespace MyRecipeBook.Application.UseCases.Recipe.Update;

public class UpdateRecipeByIdUseCase : IUpdateRecipeByIdUseCase
{
    private readonly IRecipeUpdateOnlyRepository  _recipeUpdateOnlyRepository;
    private readonly IUnitOfWork  _unitOfWork;
    private readonly ILoggedUser  _loggedUser;

    public UpdateRecipeByIdUseCase(IRecipeUpdateOnlyRepository recipeUpdateOnlyRepository, IUnitOfWork unitOfWork, ILoggedUser loggedUser)
    {
        _recipeUpdateOnlyRepository = recipeUpdateOnlyRepository;
        _unitOfWork = unitOfWork;
        _loggedUser = loggedUser;
    }
    
    public async Task Execute(Guid id, RequestRecipeJson request)
    {
        Validate(request);
        var recipe = await _recipeUpdateOnlyRepository.GetById(id, _loggedUser.GetUserId());
        if (recipe is null)
            throw new NotFoundException(ResourceMessagesException.VALIDATION_RECIPE_NOT_FOUND);

        request.Adapt(recipe);
        
        await _unitOfWork.CommitAsync();
    }

    private void Validate(RequestRecipeJson request)
    {
        var result = new RecipeValidator().Validate(request);

        if (result.IsValid == false)
        {
            throw new ErrorOnValidationException(result.Errors.Select(e => e.ErrorMessage).ToList());
        }
    }
}