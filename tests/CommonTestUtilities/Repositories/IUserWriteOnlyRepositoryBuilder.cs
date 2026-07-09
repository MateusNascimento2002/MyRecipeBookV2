using Moq;
using MyRecipeBook.Domain.Interfaces.Repositories.Users;

namespace CommonTestUtilities.Repositories;

public class IUserWriteOnlyRepositoryBuilder
{
    public static IUserWriteOnlyRepository Build()
    {
        var moq = new Mock<IUserWriteOnlyRepository>();
        return moq.Object;
    }
}