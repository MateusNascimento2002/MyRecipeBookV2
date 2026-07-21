using DomainUser = MyRecipeBook.Domain.Entities.User;
namespace WebApi.Tests.Resources;

public class UserIdentityManager
{
    private readonly DomainUser _user;
    private readonly string _password;

    public UserIdentityManager(DomainUser user, string password)
    {
        _user = user;
        _password = password;
    }

    public Guid GetId() => _user.Id;
    public string GetName() => _user.Name;
    public string GetEmail() => _user.Email;
    public string GetPassword() => _password;
}