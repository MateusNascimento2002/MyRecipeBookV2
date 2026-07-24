using System.Net;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Identity;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using MyRecipeBook.Application.UseCases.User.Update;
using MyRecipeBook.Domain.Extensions;
using MyRecipeBook.Exception;
using MyRecipeBook.Exception.ExceptionBase;
using Shouldly;
using DomainUser = MyRecipeBook.Domain.Entities.User;

namespace UseCases.Tests.User.Update;

public class UpdateUserUseCaseTests
{
    [Fact]
    public async Task Success()
    {
        var (user, _) = UserBuilder.Build();

        var request = RequestUpdateUserJsonBuilder.Build();

        var useCase = CreateUseCase(user);

        await useCase.Execute(request).ShouldNotThrowAsync();

        user.Name.ShouldBe(request.Name);
        user.Email.ShouldBe(request.Email);
    }

    [Fact]
    public async Task Validate_ShouldThrowException_WhenNameIsEmpty()
    {
        var (user, _) = UserBuilder.Build();

        var request = RequestUpdateUserJsonBuilder.Build();
        request.Name = string.Empty;

        var useCase = CreateUseCase(user);

        var exception = await useCase.Execute(request).ShouldThrowAsync<ErrorOnValidationException>();

        exception.GetErrorMessages().ShouldSatisfyAllConditions(errorMessages =>
        {
            exception.GetStatusCode().ShouldBe(HttpStatusCode.BadRequest);
            errorMessages.Count.ShouldBe(1);
            errorMessages.ShouldContain(ResourceMessagesException.VALIDATION_NAME_REQUIRED);
        });

        user.Email.ShouldNotBe(request.Email);
        user.Name.ShouldNotBe(request.Name);
    }

    [Fact]
    public async Task Validate_ShouldThrowException_WhenEmailIsEmpty()
    {
        var (user, _) = UserBuilder.Build();

        var request = RequestUpdateUserJsonBuilder.Build();
        request.Email = string.Empty;

        var useCase = CreateUseCase(user);

        var exception = await useCase.Execute(request).ShouldThrowAsync<ErrorOnValidationException>();

        exception.GetErrorMessages().ShouldSatisfyAllConditions(errorMessages =>
        {
            exception.GetStatusCode().ShouldBe(HttpStatusCode.BadRequest);
            errorMessages.Count.ShouldBe(1);
            errorMessages.ShouldContain(ResourceMessagesException.VALIDATION_EMAIL_REQUIRED);
        });

        user.Email.ShouldNotBe(request.Email);
        user.Name.ShouldNotBe(request.Name);
    }

    [Fact]
    public async Task Validate_ShouldThrowException_WhenEmailAlreadyExists()
    {
        var (user, _) = UserBuilder.Build();

        var request = RequestUpdateUserJsonBuilder.Build();

        var useCase = CreateUseCase(user, request.Email);

        var exception = await useCase.Execute(request).ShouldThrowAsync<ErrorOnValidationException>();

        exception.GetErrorMessages().ShouldSatisfyAllConditions(errorMessages =>
        {
            exception.GetStatusCode().ShouldBe(HttpStatusCode.BadRequest);
            errorMessages.Count.ShouldBe(1);
            errorMessages.ShouldContain(ResourceMessagesException.VALIDATION_EMAIL_ALREADY_EXISTS);
        });

        user.Email.ShouldNotBe(request.Email);
        user.Name.ShouldNotBe(request.Name);
    }

    private static UpdateUserUseCase CreateUseCase(DomainUser user, string? emailThatAlreadyExists = "")
    {
        var unitOfWork = IUnitOfWorkBuilder.Build();
        var userUpdateOnlyRepository = IUserUpdateOnlyRepositoryBuilder.Build();
        var loggedUser = ILoggedUserBuilder.Build(user);

        var userReadOnlyRepositoryBuilder = new IUserReadOnlyRepositoryBuilder();

        if (emailThatAlreadyExists.IsNotEmpty())
            userReadOnlyRepositoryBuilder.ExistActiveUserWithEmail(emailThatAlreadyExists);

        return new UpdateUserUseCase(loggedUser, userReadOnlyRepositoryBuilder.Build(), userUpdateOnlyRepository,
            unitOfWork);
    }
}