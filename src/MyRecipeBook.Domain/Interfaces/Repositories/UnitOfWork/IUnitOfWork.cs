namespace MyRecipeBook.Domain.Interfaces.Repositories.UnitOfWork;

public interface IUnitOfWork
{
    Task CommitAsync();
}