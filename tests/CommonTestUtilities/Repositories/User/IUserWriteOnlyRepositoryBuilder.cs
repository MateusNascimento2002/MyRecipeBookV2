using Moq;
using MyRecipeBook.Domain.Interfaces.Repositories.User;

namespace CommonTestUtilities.Repositories.User;

public class IUserWriteOnlyRepositoryBuilder
{
    public static IUserWriteOnlyRepository Build()
    {
        var moq = new Mock<IUserWriteOnlyRepository>();
        return moq.Object;
    }
}