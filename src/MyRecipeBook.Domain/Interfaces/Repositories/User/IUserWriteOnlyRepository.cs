namespace MyRecipeBook.Domain.Interfaces.Repositories.User;

public interface IUserWriteOnlyRepository
{
    Task Add(Entities.User user);
}