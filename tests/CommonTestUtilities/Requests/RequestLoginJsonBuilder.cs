using Bogus;
using MyRecipeBook.Communication.Requests;

namespace CommonTestUtilities.Requests;

public class RequestLoginJsonBuilder
{
    public static RequestLoginJson Build()
    {
        return new Faker<RequestLoginJson>()
            .RuleFor(request => request.Email, (faker, user) => faker.Internet.Email())
            .RuleFor(request => request.Password, faker => faker.Internet.Password());
    }
}