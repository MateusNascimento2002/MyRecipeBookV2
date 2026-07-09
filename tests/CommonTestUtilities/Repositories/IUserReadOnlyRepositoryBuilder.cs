using Moq;
using MyRecipeBook.Domain.Interfaces.Repositories.Users;

namespace CommonTestUtilities.Repositories;

public class IUserReadOnlyRepositoryBuilder
{
    private readonly Mock<IUserReadOnlyRepository> _userReadOnlyRepositoryMock;

    public IUserReadOnlyRepositoryBuilder()
    {
        _userReadOnlyRepositoryMock = new Mock<IUserReadOnlyRepository>();
    }

    public void ExistActiveUserWithEmail(string email)
    {
        _userReadOnlyRepositoryMock.Setup(repo => repo.ExistActiveUserWithEmail(email)).ReturnsAsync(false);
    }
    public IUserReadOnlyRepository Build()
    {
        return _userReadOnlyRepositoryMock.Object;
    }
}