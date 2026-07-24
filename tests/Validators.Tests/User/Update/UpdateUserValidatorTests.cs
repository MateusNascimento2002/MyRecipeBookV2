using System.Diagnostics.CodeAnalysis;
using CommonTestUtilities.Requests;
using MyRecipeBook.Application.UseCases.User.Update;
using MyRecipeBook.Exception;
using Shouldly;

namespace Validators.Tests.User.Update;

[SuppressMessage("Usage", "xUnit1012:Null should only be used for nullable parameters")]
public class UpdateUserValidatorTests
{
    [Fact]
    public void Success()
    {
        var request = RequestUpdateUserJsonBuilder.Build();

        var validator = new UpdateUserValidator();

        var result = validator.Validate(request);

        result.IsValid.ShouldBeTrue();
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("    ")]
    public void Validate_ShouldHaveError_WhenNameIsEmpty(string name)
    {
        var request = RequestUpdateUserJsonBuilder.Build();
        request.Name = name;

        var validator = new UpdateUserValidator();

        var result = validator.Validate(request);

        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldSatisfyAllConditions(errors =>
        {
            errors.ShouldHaveSingleItem();
            errors.ShouldContain(error =>
                error.ErrorMessage.Equals(ResourceMessagesException.VALIDATION_NAME_REQUIRED));
        });
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("    ")]
    public void Validate_ShouldHaveError_WhenEmailIsEmpty(string email)
    {
        var request = RequestUpdateUserJsonBuilder.Build();
        request.Email = email;

        var validator = new UpdateUserValidator();

        var result = validator.Validate(request);

        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldSatisfyAllConditions(errors =>
        {
            errors.ShouldHaveSingleItem();
            errors.ShouldContain(error =>
                error.ErrorMessage.Equals(ResourceMessagesException.VALIDATION_EMAIL_REQUIRED));
        });
    }

    [Fact]
    public void Validate_ShouldHaveError_WhenEmailIsInvalid()
    {
        // Arrange
        var request = RequestUpdateUserJsonBuilder.Build();
        request.Email = "invalid-email";

        var validator = new UpdateUserValidator();

        // Act
        var result = validator.Validate(request);

        // Assert
        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldSatisfyAllConditions(errors =>
        {
            errors.ShouldHaveSingleItem();
            errors.ShouldContain(error =>
                error.ErrorMessage.Equals(ResourceMessagesException.VALIDATION_EMAIL_INVALID));
            errors.ShouldContain(error => error.PropertyName.Equals(nameof(request.Email)));
        });
    }
}