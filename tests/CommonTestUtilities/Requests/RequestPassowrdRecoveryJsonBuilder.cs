using Bogus;
using MyRecipeBook.Communication.Requests;

namespace CommonTestUtilities.Requests;

public class RequestPassowrdRecoveryJsonBuilder
{
    public static RequestPasswordRecoveryJson Build()
    {
        return new Faker<RequestPasswordRecoveryJson>()
            .RuleFor(request => request.Email, faker => faker.Internet.Email());
    }
}