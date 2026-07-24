using DomainUser = MyRecipeBook.Domain.Entities.User;

namespace WebApi.Tests.Resources;

public class UserIdentityManager
{
    private readonly string _accessToken;
    private readonly string _password;
    private readonly DomainUser _user;

    public UserIdentityManager(DomainUser user, string password, string accessToken)
    {
        _user = user;
        _password = password;
        _accessToken = accessToken;
    }

    public Guid GetId()
    {
        return _user.Id;
    }

    public string GetName()
    {
        return _user.Name;
    }

    public string GetEmail()
    {
        return _user.Email;
    }

    public string GetPassword()
    {
        return _password;
    }

    public string GetAccessToken()
    {
        return _accessToken;
    }
}