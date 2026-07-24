using System.Diagnostics.CodeAnalysis;
using CommonTestUtilities.Requests;
using MyRecipeBook.Application.UseCases.User.ChangePassword;
using MyRecipeBook.Exception;
using Shouldly;

namespace Validators.Tests.User.ChangePassword;

[SuppressMessage("Usage", "xUnit1012:Null should only be used for nullable parameters")]
public class ChangePasswordValidatorTests
{
    [Fact]
    public void Success()
    {
        var request = RequestChangePasswordJsonBuilder.Build();

        var validator = new ChangePasswordValidator();

        var result = validator.Validate(request);

        result.IsValid.ShouldBeTrue();
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("    ")]
    public void Validate_ShouldHaveError_WhenNewPasswordIsEmpty(string password)
    {
        var request = RequestChangePasswordJsonBuilder.Build();
        request.NewPassword = password;

        var validator = new ChangePasswordValidator();

        var result = validator.Validate(request);

        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldSatisfyAllConditions(errors =>
        {
            errors.ShouldHaveSingleItem();
            errors.ShouldContain(error =>
                error.ErrorMessage.Equals(ResourceMessagesException.VALIDATION_PASSWORD_REQUIRED));
        });
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    [InlineData(6)]
    [InlineData(7)]
    public void Validate_ShouldHaveError_WhenNewPasswordIsInvalid(int passwordLength)
    {
        // Arrange
        var request = RequestChangePasswordJsonBuilder.Build(passwordLength);

        var validator = new ChangePasswordValidator();

        // Act
        var result = validator.Validate(request);

        // Assert
        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldSatisfyAllConditions(errors =>
        {
            errors.ShouldHaveSingleItem();
            errors.ShouldContain(error =>
                error.ErrorMessage.Equals(ResourceMessagesException.VALIDATION_PASSWORD_MIN_LENGTH));
            errors.ShouldContain(error => error.PropertyName.Equals(nameof(request.NewPassword)));
        });
    }
}