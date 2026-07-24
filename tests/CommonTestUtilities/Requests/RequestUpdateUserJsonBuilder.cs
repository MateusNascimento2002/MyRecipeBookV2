using Bogus;
using MyRecipeBook.Communication.Requests;

namespace CommonTestUtilities.Requests;

public class RequestUpdateUserJsonBuilder
{
    public static RequestUpdateUserJson Build()
    {
        return new Faker<RequestUpdateUserJson>()
            .RuleFor(x => x.Name, f => f.Name.FirstName())
            .RuleFor(x => x.Email, (f, u) => f.Internet.Email(u.Name));
    }
}