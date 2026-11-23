namespace MyRecipeBook.Domain.Interfaces.Repositories.VerificationCode;

public interface IVerificationCodeReadOnlyRepository
{
    Task<Entities.VerificationCode?> Get(Guid userId, string code, Enums.VerificationCode.VerificationCodeType type);
}