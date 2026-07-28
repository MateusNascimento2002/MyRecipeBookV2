using Moq;
using MyRecipeBook.Domain.Interfaces.Repositories.User;

namespace CommonTestUtilities.Repositories.User;

public class IUserReadOnlyRepositoryBuilder
{
    private readonly Mock<IUserReadOnlyRepository> _userReadOnlyRepositoryMock;

    public IUserReadOnlyRepositoryBuilder()
    {
        _userReadOnlyRepositoryMock = new Mock<IUserReadOnlyRepository>();
    }

    public void ExistActiveUserWithEmail(string email)
    {
        _userReadOnlyRepositoryMock.Setup(repo => repo.ExistActiveUserWithEmail(email)).ReturnsAsync(true);
    }

    public void GetUserByEmail(MyRecipeBook.Domain.Entities.User user)
    {
        _userReadOnlyRepositoryMock.Setup(repo => repo.GetByEmail(user.Email)).ReturnsAsync(user);
    }

    public IUserReadOnlyRepository Build()
    {
        return _userReadOnlyRepositoryMock.Object;
    }
}