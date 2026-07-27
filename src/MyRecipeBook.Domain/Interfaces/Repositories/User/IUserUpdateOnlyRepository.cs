namespace MyRecipeBook.Domain.Interfaces.Repositories.User;

public interface IUserUpdateOnlyRepository
{
    void UpdateProfile(Entities.User user);
    Task UpdatePassword(Guid userId, string newPassword);
}