using Moq;
using MyRecipeBook.Domain.Interfaces.Repositories.Recipe;

namespace CommonTestUtilities.Repositories.Recipe;

public class IRecipeWriteOnlyRepositoryBuilder
{
    public static IRecipeWriteOnlyRepository Build()
    {
        var moq = new Mock<IRecipeWriteOnlyRepository>();
        return moq.Object;
    }
}
