using Bogus;
using MyRecipeBook.Communication.Requests;

namespace CommonTestUtilities.Requests;

public class RequestChangePasswordJsonBuilder
{
    public static RequestChangePasswordJson Build(int lenght = 10)
    {
        return new Faker<RequestChangePasswordJson>()
            .RuleFor(x => x.CurrentPassword, f => f.Internet.Password())
            .RuleFor(x => x.NewPassword, f => f.Internet.Password(lenght));
    }
}