using Bogus;
using MyRecipeBook.Communication.Enums.Recipe;
using MyRecipeBook.Communication.Requests;

namespace CommonTestUtilities.Requests;

public class RequestRecipeJsonBuilder
{
    public static RequestRecipeJson Build()
    {
        return new Faker<RequestRecipeJson>()
            .RuleFor(request => request.Title, faker => faker.Lorem.Word())
            .RuleFor(request => request.CookTime, faker => faker.PickRandom<CookTime>())
            .RuleFor(request => request.Description, faker => faker.Lorem.Sentence())
            .RuleFor(request => request.Ingredients, faker => faker.Make(3, () => faker.Commerce.ProductName()))
            .RuleFor(request => request.Instructions, faker => faker
                .Make(3, () => faker.Lorem.Sentence())
                .Select((description, index) => new RequestInstructionJson
                {
                    Order = index,
                    Description = description
                })
                .ToList())
            .RuleFor(request => request.DishTypes, faker => new List<DishType> { faker.PickRandom<DishType>() });
    }
}
