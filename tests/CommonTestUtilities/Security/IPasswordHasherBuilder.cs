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

    public void VerifyHashedPassword(string password)
    {
        _passwordHasherMock.Setup(repo => repo.VerifyHashedPassword(password, It.IsAny<string>())).Returns(true);
    }

    public IPasswordHasher Build()
    {
        return _passwordHasherMock.Object;
    }
}