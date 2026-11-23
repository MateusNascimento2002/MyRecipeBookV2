namespace MyRecipeBook.Domain.Interfaces.Repositories.VerificationCode;

public interface IVerificationCodeWriteOnlyRepository
{
    Task AddAsync(Entities.VerificationCode code);
}