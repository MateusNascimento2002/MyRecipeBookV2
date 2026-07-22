using MyRecipeBook.Domain.Entities;

namespace MyRecipeBook.Domain.Interfaces.Repositories.Users;

public interface IUserReadOnlyRepository
{
    Task<bool> ExistActiveUserWithEmail(string email);
    Task<bool> ExistActiveUserWithId(Guid id);
    Task<User?> GetByEmail(string email);
}