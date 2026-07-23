using MyRecipeBook.Domain.Entities;

namespace MyRecipeBook.Domain.Interfaces.Repositories.Users;

public interface IUserUpdateOnlyRepository
{
    void UpdateProfile(User user);
    Task UpdatePassword(Guid userId, string newPassword);
}