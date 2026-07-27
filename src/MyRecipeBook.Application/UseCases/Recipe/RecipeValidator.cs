using FluentValidation;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Exception;

namespace MyRecipeBook.Application.UseCases.Recipe;

public class RecipeValidator : AbstractValidator<RequestRecipeJson>
{
    public RecipeValidator()
    {
        RuleFor(x => x.Title)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage(ResourceMessagesException.VALIDATION_RECIPE_TITLE_REQUIRED)
            .MaximumLength(256)
            .WithMessage(ResourceMessagesException.VALIDATION_RECIPE_TITLE_MAX_LENGTH);

        RuleFor(x => x.Description)
            .MaximumLength(512)
            .WithMessage(ResourceMessagesException.VALIDATION_RECIPE_DESCRIPTION_MAX_LENGTH);

        RuleFor(x => x.CookTime)
            .Cascade(CascadeMode.Stop)
            .IsInEnum()
            .WithMessage(ResourceMessagesException.VALIDATION_RECIPE_COOK_TIME_INVALID);

        RuleFor(x => x.DishTypes)
            .NotEmpty()
            .WithMessage(ResourceMessagesException.VALIDATION_RECIPE_DISH_TYPE_REQUIRED);
        
        RuleForEach(x => x.DishTypes)
            .IsInEnum()
            .WithMessage(ResourceMessagesException.VALIDATION_RECIPE_DISH_TYPE_INVALID);

        RuleFor(x => x.Ingredients)
            .NotEmpty()
            .WithMessage(ResourceMessagesException.VALIDATION_RECIPE_INGREDIENTS_REQUIRED);

        RuleForEach(x => x.Ingredients)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage(ResourceMessagesException.VALIDATION_RECIPE_INGREDIENT_REQUIRED)
            .MaximumLength(512)
            .WithMessage(ResourceMessagesException.VALIDATION_RECIPE_INGREDIENT_MAX_LENGTH);

        RuleFor(x => x.Instructions)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage(ResourceMessagesException.VALIDATION_RECIPE_INSTRUCTIONS_REQUIRED)
            .Must(instructions => instructions.Select(instruction => instruction.Order).Distinct().Count() == instructions.Count)
            .WithMessage(ResourceMessagesException.VALIDATION_RECIPE_INSTRUCTION_ORDER_DUPLICATED);

        RuleForEach(x => x.Instructions).ChildRules(instruction =>
        {
            instruction.RuleFor(x => x.Order)
                .GreaterThanOrEqualTo(0)
                .WithMessage(ResourceMessagesException.VALIDATION_RECIPE_INSTRUCTION_ORDER_INVALID);

            instruction.RuleFor(x => x.Description)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithMessage(ResourceMessagesException.VALIDATION_RECIPE_INSTRUCTION_REQUIRED)
                .MaximumLength(512)
                .WithMessage(ResourceMessagesException.VALIDATION_RECIPE_INSTRUCTION_MAX_LENGTH);
        });
    }
}
