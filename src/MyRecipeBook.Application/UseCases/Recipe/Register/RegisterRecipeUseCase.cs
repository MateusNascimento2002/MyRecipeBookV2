using Mapster;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Communication.Responses;
using MyRecipeBook.Domain.Identity;
using MyRecipeBook.Domain.Interfaces.Repositories.Recipe;
using MyRecipeBook.Domain.Interfaces.Repositories.UnitOfWork;
using MyRecipeBook.Exception.ExceptionBase;
using DomainRecipe = MyRecipeBook.Domain.Entities.Recipe;
namespace MyRecipeBook.Application.UseCases.Recipe.Register;

public class RegisterRecipeUseCase : IRegisterRecipeUseCase
{
    private readonly IRecipeWriteOnlyRepository  _recipeWriteOnlyRepository;
    private readonly IUnitOfWork  _unitOfWork;
    private readonly ILoggedUser  _loggedUser;

    public RegisterRecipeUseCase(IRecipeWriteOnlyRepository recipeWriteOnlyRepository, IUnitOfWork unitOfWork, ILoggedUser loggedUser)
    {
        _recipeWriteOnlyRepository = recipeWriteOnlyRepository;
        _unitOfWork = unitOfWork;
        _loggedUser = loggedUser;
    }
    
    public async Task<ResponseRegisteredRecipeJson> Execute(RequestRecipeJson request)
    {
        Validate(request);
        
        var recipe = request.Adapt<DomainRecipe>();
        
        recipe.UserId = _loggedUser.GetUserId();
        
        await _recipeWriteOnlyRepository.Add(recipe);
        
        await _unitOfWork.CommitAsync();
        
        return recipe.Adapt<ResponseRegisteredRecipeJson>();
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