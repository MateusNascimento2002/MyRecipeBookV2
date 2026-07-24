using Moq;
using MyRecipeBook.Domain.Security.PasswordHashing;

namespace CommonTestUtilities.Repositories;

public class IPasswordHasherBuilder
{
    private readonly Mock<IPasswordHasher> _passwordHasherMock;

    public IPasswordHasherBuilder()
    {
        _passwordHasherMock = new Mock<IPasswordHasher>();

        _passwordHasherMock.Setup(repo => repo.HashPassword(It.IsAny<string>())).Returns("hashedPassword");
    }

    public IPasswordHasherBuilder VerifyHashedPassword(string password)
    {
        _passwordHasherMock.Setup(repo => repo.VerifyPassword(password, It.IsAny<string>())).Returns(true);

        return this;
    }

    public IPasswordHasher Build()
    {
        return _passwordHasherMock.Object;
    }
}