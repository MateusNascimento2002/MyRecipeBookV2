using System.Diagnostics.CodeAnalysis;
using CommonTestUtilities.Requests;
using MyRecipeBook.Application.UseCases.Recipe;
using MyRecipeBook.Communication.Enums.Recipe;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Exception;
using Shouldly;

namespace Validators.Tests.Recipe;

[SuppressMessage("Usage", "xUnit1012:Null should only be used for nullable parameters")]
public class RecipeValidatorTests
{
    [Fact]
    public void Success()
    {
        var request = RequestRecipeJsonBuilder.Build();

        var validator = new RecipeValidator();

        var result = validator.Validate(request);

        result.IsValid.ShouldBeTrue();
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("    ")]
    public void Validate_ShouldHaveError_WhenTitleIsEmpty(string title)
    {
        var request = RequestRecipeJsonBuilder.Build();
        request.Title = title;

        var validator = new RecipeValidator();

        var result = validator.Validate(request);

        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldSatisfyAllConditions(errors =>
        {
            errors.ShouldHaveSingleItem();
            errors.ShouldContain(error =>
                error.ErrorMessage.Equals(ResourceMessagesException.VALIDATION_RECIPE_TITLE_REQUIRED));
        });
    }

    [Fact]
    public void Validate_ShouldHaveError_WhenTitleIsTooLong()
    {
        // Arrange
        var request = RequestRecipeJsonBuilder.Build();
        request.Title = new string('a', 257);

        var validator = new RecipeValidator();

        // Act
        var result = validator.Validate(request);

        // Assert
        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldSatisfyAllConditions(errors =>
        {
            errors.ShouldHaveSingleItem();
            errors.ShouldContain(error =>
                error.ErrorMessage.Equals(ResourceMessagesException.VALIDATION_RECIPE_TITLE_MAX_LENGTH));
            errors.ShouldContain(error => error.PropertyName.Equals(nameof(request.Title)));
        });
    }

    [Fact]
    public void Validate_ShouldHaveError_WhenDescriptionIsTooLong()
    {
        // Arrange
        var request = RequestRecipeJsonBuilder.Build();
        request.Description = new string('a', 513);

        var validator = new RecipeValidator();

        // Act
        var result = validator.Validate(request);

        // Assert
        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldSatisfyAllConditions(errors =>
        {
            errors.ShouldHaveSingleItem();
            errors.ShouldContain(error =>
                error.ErrorMessage.Equals(ResourceMessagesException.VALIDATION_RECIPE_DESCRIPTION_MAX_LENGTH));
            errors.ShouldContain(error => error.PropertyName.Equals(nameof(request.Description)));
        });
    }

    [Fact]
    public void Validate_ShouldHaveError_WhenCookTimeIsInvalid()
    {
        // Arrange
        var request = RequestRecipeJsonBuilder.Build();
        request.CookTime = (CookTime)999;

        var validator = new RecipeValidator();

        // Act
        var result = validator.Validate(request);

        // Assert
        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldSatisfyAllConditions(errors =>
        {
            errors.ShouldHaveSingleItem();
            errors.ShouldContain(error =>
                error.ErrorMessage.Equals(ResourceMessagesException.VALIDATION_RECIPE_COOK_TIME_INVALID));
            errors.ShouldContain(error => error.PropertyName.Equals(nameof(request.CookTime)));
        });
    }

    [Fact]
    public void Validate_ShouldHaveError_WhenDishTypesIsEmpty()
    {
        var request = RequestRecipeJsonBuilder.Build();
        request.DishTypes = [];

        var validator = new RecipeValidator();

        var result = validator.Validate(request);

        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldSatisfyAllConditions(errors =>
        {
            errors.ShouldHaveSingleItem();
            errors.ShouldContain(error =>
                error.ErrorMessage.Equals(ResourceMessagesException.VALIDATION_RECIPE_DISH_TYPE_REQUIRED));
        });
    }

    [Fact]
    public void Validate_ShouldHaveError_WhenDishTypeIsInvalid()
    {
        // Arrange
        var request = RequestRecipeJsonBuilder.Build();
        request.DishTypes = [(DishType)999];

        var validator = new RecipeValidator();

        // Act
        var result = validator.Validate(request);

        // Assert
        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldSatisfyAllConditions(errors =>
        {
            errors.ShouldHaveSingleItem();
            errors.ShouldContain(error =>
                error.ErrorMessage.Equals(ResourceMessagesException.VALIDATION_RECIPE_DISH_TYPE_INVALID));
        });
    }

    [Fact]
    public void Validate_ShouldHaveError_WhenIngredientsIsEmpty()
    {
        var request = RequestRecipeJsonBuilder.Build();
        request.Ingredients = [];

        var validator = new RecipeValidator();

        var result = validator.Validate(request);

        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldSatisfyAllConditions(errors =>
        {
            errors.ShouldHaveSingleItem();
            errors.ShouldContain(error =>
                error.ErrorMessage.Equals(ResourceMessagesException.VALIDATION_RECIPE_INGREDIENTS_REQUIRED));
        });
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("    ")]
    public void Validate_ShouldHaveError_WhenIngredientIsEmpty(string ingredient)
    {
        var request = RequestRecipeJsonBuilder.Build();
        request.Ingredients = [ingredient];

        var validator = new RecipeValidator();

        var result = validator.Validate(request);

        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldSatisfyAllConditions(errors =>
        {
            errors.ShouldHaveSingleItem();
            errors.ShouldContain(error =>
                error.ErrorMessage.Equals(ResourceMessagesException.VALIDATION_RECIPE_INGREDIENT_REQUIRED));
        });
    }

    [Fact]
    public void Validate_ShouldHaveError_WhenIngredientIsTooLong()
    {
        // Arrange
        var request = RequestRecipeJsonBuilder.Build();
        request.Ingredients = [new string('a', 513)];

        var validator = new RecipeValidator();

        // Act
        var result = validator.Validate(request);

        // Assert
        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldSatisfyAllConditions(errors =>
        {
            errors.ShouldHaveSingleItem();
            errors.ShouldContain(error =>
                error.ErrorMessage.Equals(ResourceMessagesException.VALIDATION_RECIPE_INGREDIENT_MAX_LENGTH));
        });
    }

    [Fact]
    public void Validate_ShouldHaveError_WhenInstructionsIsEmpty()
    {
        var request = RequestRecipeJsonBuilder.Build();
        request.Instructions = [];

        var validator = new RecipeValidator();

        var result = validator.Validate(request);

        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldSatisfyAllConditions(errors =>
        {
            errors.ShouldHaveSingleItem();
            errors.ShouldContain(error =>
                error.ErrorMessage.Equals(ResourceMessagesException.VALIDATION_RECIPE_INSTRUCTIONS_REQUIRED));
        });
    }

    [Fact]
    public void Validate_ShouldHaveError_WhenInstructionsHaveDuplicatedOrder()
    {
        // Arrange
        var request = RequestRecipeJsonBuilder.Build();
        request.Instructions =
        [
            new RequestInstructionJson { Order = 1, Description = "Mix the ingredients." },
            new RequestInstructionJson { Order = 1, Description = "Bake for 30 minutes." }
        ];

        var validator = new RecipeValidator();

        // Act
        var result = validator.Validate(request);

        // Assert
        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldSatisfyAllConditions(errors =>
        {
            errors.ShouldHaveSingleItem();
            errors.ShouldContain(error =>
                error.ErrorMessage.Equals(ResourceMessagesException.VALIDATION_RECIPE_INSTRUCTION_ORDER_DUPLICATED));
        });
    }

    [Fact]
    public void Validate_ShouldHaveError_WhenInstructionOrderIsNegative()
    {
        // Arrange
        var request = RequestRecipeJsonBuilder.Build();
        request.Instructions = [new RequestInstructionJson { Order = -1, Description = "Mix the ingredients." }];

        var validator = new RecipeValidator();

        // Act
        var result = validator.Validate(request);

        // Assert
        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldSatisfyAllConditions(errors =>
        {
            errors.ShouldHaveSingleItem();
            errors.ShouldContain(error =>
                error.ErrorMessage.Equals(ResourceMessagesException.VALIDATION_RECIPE_INSTRUCTION_ORDER_INVALID));
        });
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("    ")]
    public void Validate_ShouldHaveError_WhenInstructionDescriptionIsEmpty(string description)
    {
        var request = RequestRecipeJsonBuilder.Build();
        request.Instructions = [new RequestInstructionJson { Order = 1, Description = description }];

        var validator = new RecipeValidator();

        var result = validator.Validate(request);

        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldSatisfyAllConditions(errors =>
        {
            errors.ShouldHaveSingleItem();
            errors.ShouldContain(error =>
                error.ErrorMessage.Equals(ResourceMessagesException.VALIDATION_RECIPE_INSTRUCTION_REQUIRED));
        });
    }

    [Fact]
    public void Validate_ShouldHaveError_WhenInstructionDescriptionIsTooLong()
    {
        // Arrange
        var request = RequestRecipeJsonBuilder.Build();
        request.Instructions = [new RequestInstructionJson { Order = 1, Description = new string('a', 513) }];

        var validator = new RecipeValidator();

        // Act
        var result = validator.Validate(request);

        // Assert
        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldSatisfyAllConditions(errors =>
        {
            errors.ShouldHaveSingleItem();
            errors.ShouldContain(error =>
                error.ErrorMessage.Equals(ResourceMessagesException.VALIDATION_RECIPE_INSTRUCTION_MAX_LENGTH));
        });
    }
}
