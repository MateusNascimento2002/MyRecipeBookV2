using Moq;
using MyRecipeBook.Domain.Interfaces.Repositories.Users;

namespace CommonTestUtilities.Repositories;

public class IUserUpdateOnlyRepositoryBuilder
{
    public static IUserUpdateOnlyRepository Build()
    {
        var mock = new Mock<IUserUpdateOnlyRepository>();
        return mock.Object;
    }
}