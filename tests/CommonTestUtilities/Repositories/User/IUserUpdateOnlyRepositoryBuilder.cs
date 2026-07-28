using Moq;
using MyRecipeBook.Domain.Interfaces.Repositories.User;

namespace CommonTestUtilities.Repositories.User;

public class IUserUpdateOnlyRepositoryBuilder
{
    public static IUserUpdateOnlyRepository Build()
    {
        var mock = new Mock<IUserUpdateOnlyRepository>();
        return mock.Object;
    }
}