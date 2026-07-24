using Microsoft.EntityFrameworkCore;
using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Domain.Interfaces.Repositories.Users;

namespace MyRecipeBook.Infrastructure.DataAccess.Repositories;

internal class UserRepository : IUserWriteOnlyRepository, IUserReadOnlyRepository, IUserUpdateOnlyRepository
{
    private readonly MyRecipeBookDbContext _context;

    public UserRepository(MyRecipeBookDbContext context)
    {
        _context = context;
    }

    public Task<bool> ExistActiveUserWithEmail(string email)
    {
        return _context.Users.AnyAsync(u => u.Email.Equals(email) && u.IsActive);
    }

    public Task<bool> ExistActiveUserWithId(Guid id)
    {
        return _context.Users.AnyAsync(u => u.Id == id && u.IsActive);
    }

    public async Task<User?> GetByEmail(string email)
    {
        return await _context.Users
            .AsNoTracking()
            .SingleOrDefaultAsync(u => u.IsActive && u.Email.Equals(email));
    }

    public void UpdateProfile(User user)
    {
        _context.Users.Attach(user);

        _context.Entry(user).Property(u => u.Name).IsModified = true;
        _context.Entry(user).Property(u => u.Email).IsModified = true;
    }

    public async Task UpdatePassword(Guid userId, string newPassword)
    {
        await _context
            .Users
            .Where(user => user.Id == userId)
            .ExecuteUpdateAsync(setter =>
                setter.SetProperty(user => user.Password, newPassword)
            );
    }

    public async Task Add(User user)
    {
        await _context.Users.AddAsync(user);
    }
}