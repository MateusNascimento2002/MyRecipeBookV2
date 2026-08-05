namespace MyRecipeBook.Domain.Interfaces.Repositories.VerificationCode;

public interface IVerificationCodeWriteOnlyRepository
{
    Task AddAsync(Domain.Entities.VerificationCode code);
}