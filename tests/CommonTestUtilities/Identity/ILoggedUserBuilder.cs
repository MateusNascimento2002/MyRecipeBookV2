using Moq;
using MyRecipeBook.Domain.Identity;
using DomainUser = MyRecipeBook.Domain.Entities.User;

namespace CommonTestUtilities.Identity;

public class ILoggedUserBuilder
{
    public static ILoggedUser Build(DomainUser user)
    {
        var mock = new Mock<ILoggedUser>();

        mock.Setup(loggedUser => loggedUser.Get()).ReturnsAsync(user);

        mock.Setup(loggedUser => loggedUser.GetUserId()).Returns(user.Id);

        return mock.Object;
    }
}