namespace MyRecipeBook.Domain.Interfaces.Repositories.VerificationCode;

public interface IVerificationCodeWriteOnlyRepository
{
    Task Replace(Entities.VerificationCode code);
    Task Delete(Entities.VerificationCode code);
}