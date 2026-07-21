using MyRecipeBook.Domain.Interfaces.Repositories.UnitOfWork;

namespace MyRecipeBook.Infrastructure.DataAccess;

internal class UnitOfWork : IUnitOfWork
{
    private readonly MyRecipeBookDbContext _context;

    public UnitOfWork(MyRecipeBookDbContext context)
    {
        _context = context;
    }

    public async Task CommitAsync()
    {
        await _context.SaveChangesAsync();
    }
}