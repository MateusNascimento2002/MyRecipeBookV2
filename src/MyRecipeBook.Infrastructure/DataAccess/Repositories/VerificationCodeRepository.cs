using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Domain.Interfaces.Repositories.VerificationCode;

namespace MyRecipeBook.Infrastructure.DataAccess.Repositories;

internal class VerificationCodeRepository : IVerificationCodeWriteOnlyRepository
{
    private readonly MyRecipeBookDbContext _context;

    public VerificationCodeRepository(MyRecipeBookDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(VerificationCode code)
    {
        await _context.VerificationCodes.AddAsync(code);
    }
}