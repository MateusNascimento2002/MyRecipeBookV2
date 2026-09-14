using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Repositories.User;
using CommonTestUtilities.Repositories.VerificationCode;
using CommonTestUtilities.Requests;
using MyRecipeBook.Application.UseCases.PasswordRecovery;
using MyRecipeBook.Communication.Requests;
using Shouldly;

namespace UseCases.Tests.PasswordRecovery;

public class RequestPasswordRecoveryCodeUseCaseTests
{
    [Fact]
    public async Task Success_WhenUserExists()
    {
        var (user, _) = UserBuilder.Build();

        var request = new RequestPasswordRecoveryJson()
        {
            Email = user.Email,
        };
        
        var useCase = CreateUseCase(user);

        await useCase.Execute(request).ShouldNotThrowAsync();
    }
    
    [Fact]
    public async Task Success_WhenUserDoesNotExists()
    {
        var request = RequestPassowrdRecoveryJsonBuilder.Build();
        
        var useCase = CreateUseCase();

        await useCase.Execute(request).ShouldNotThrowAsync();
    }
    
    private static RequestPasswordRecoveryCodeUseCase CreateUseCase(MyRecipeBook.Domain.Entities.User? user = null)
    {
        var unitOfWork = IUnitOfWorkBuilder.Build();
        var verificationCodeWriteOnlyRepository = IVerificationCodeWriteOnlyRepositoryBuilder.Build();
        var userReadonlyRepository = new IUserReadOnlyRepositoryBuilder();

        if (user is not null)
            userReadonlyRepository.GetUserByEmail(user);

        return new RequestPasswordRecoveryCodeUseCase(userReadonlyRepository.Build(), verificationCodeWriteOnlyRepository, unitOfWork);
    }
}