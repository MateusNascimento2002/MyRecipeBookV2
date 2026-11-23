using Microsoft.EntityFrameworkCore;
using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Domain.Enums.VerificationCode;
using MyRecipeBook.Domain.Interfaces.Repositories.VerificationCode;

namespace MyRecipeBook.Infrastructure.DataAccess.Repositories;

internal class VerificationCodeRepository : IVerificationCodeWriteOnlyRepository, IVerificationCodeReadOnlyRepository
{
    private readonly MyRecipeBookDbContext _context;

    public VerificationCodeRepository(MyRecipeBookDbContext context)
    {
        _context = context;
    }

    public async Task Replace(VerificationCode code)
    {
        var existingCodes = await _context.VerificationCodes
            .Where(vc => vc.UserId == code.UserId && vc.Type == code.Type)
            .ToListAsync();
        
        _context.VerificationCodes.RemoveRange(existingCodes);
        
        await _context.VerificationCodes.AddAsync(code);
    }

    public async Task Delete(VerificationCode code)
    {
        await _context.VerificationCodes.Where(vc => vc.Id == code.Id).ExecuteDeleteAsync();
    }

    public async Task<VerificationCode?> Get(Guid userId, string code, VerificationCodeType type)
    {
        return await _context.VerificationCodes
            .Where(vc => vc.UserId == userId && vc.Code == code && vc.Type == type)
            .SingleOrDefaultAsync();
    }
}