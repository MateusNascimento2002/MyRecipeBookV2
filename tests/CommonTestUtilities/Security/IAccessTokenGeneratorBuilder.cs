using Bogus;
using Moq;
using MyRecipeBook.Domain.Security.Tokens;

namespace CommonTestUtilities.Security;

public class IAccessTokenGeneratorBuilder
{
    public static IAccessTokenGenerator Build()
    {
        var moq = new Mock<IAccessTokenGenerator>();
        var fakeToken =
            new Faker().Random.String2(32, "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789");
        moq.Setup(generator => generator.Generate(It.IsAny<MyRecipeBook.Domain.Entities.User>())).Returns(fakeToken);
        return moq.Object;
    }
}