using MyRecipeBook.Domain.Entities;

namespace MyRecipeBook.Domain.Interfaces.Repositories.Users;

public interface IUserReadOnlyRepository
{
    Task<bool> ExistActiveUserWithEmail(string email);
    Task<User?> GetByEmail(string email);
}