using MyRecipeBook.Domain.Entities;

namespace MyRecipeBook.Domain.Interfaces.Repositories.Users;

public interface IUserWriteOnlyRepository
{
    Task Add(User user);
}