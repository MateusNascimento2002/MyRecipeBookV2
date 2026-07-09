using Moq;
using MyRecipeBook.Domain.Interfaces.Repositories.UnitOfWork;

namespace CommonTestUtilities.Repositories;

public class IUnitOfWorkBuilder
{
    public static IUnitOfWork Build()
    {
        var moq = new Mock<IUnitOfWork>();
        return moq.Object;
    }
}