using System.Runtime.CompilerServices;
using Mapster;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Communication.Responses;
using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Domain.Enums.Recipe;
using DomainRecipe = MyRecipeBook.Domain.Entities.Recipe;
using DomainUser = MyRecipeBook.Domain.Entities.User;

[assembly: InternalsVisibleTo("UseCases.Tests")]

namespace MyRecipeBook.Application.Mappings;

internal static class MapsterConfiguration
{
    internal static void Configure()
    {
        TypeAdapterConfig<RequestRegisterUserAccountJson, DomainUser>
            .NewConfig()
            .Ignore(destination => destination.Password);

        TypeAdapterConfig<RequestRecipeJson, DomainRecipe>
            .NewConfig()
            .Map(destination => destination.Ingredients, source => source.Ingredients.Select(ingredient =>
                    new RecipeIngredient()
                    {
                        Item = ingredient
                    }
                )
            )
            .Map(destination => destination.DishTypes, source => source.DishTypes.Select(dishType =>
                    new RecipeDishType()
                    {
                        Type = (DishType)dishType
                    }
                )
            );

        TypeAdapterConfig<DomainRecipe, ResponseRecipeJson>
            .NewConfig()
            .Map(destination => destination.Ingredients,
                source => source.Ingredients.Select(ingredient => ingredient.Item))
            .Map(destination => destination.DishTypes,
                source => source.DishTypes.Select(dishType => dishType.Type));
    }
}