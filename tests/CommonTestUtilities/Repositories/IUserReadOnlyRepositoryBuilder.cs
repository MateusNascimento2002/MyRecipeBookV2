using Moq;
using MyRecipeBook.Domain.Entities;
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
        _userReadOnlyRepositoryMock.Setup(repo => repo.ExistActiveUserWithEmail(email)).ReturnsAsync(true);
    }
    
    public void GetUserByEmail(User user)
    {
        _userReadOnlyRepositoryMock.Setup(repo => repo.GetByEmail(user.Email)).ReturnsAsync(user);
    }
    
    public IUserReadOnlyRepository Build()
    {
        return _userReadOnlyRepositoryMock.Object;
    }
}