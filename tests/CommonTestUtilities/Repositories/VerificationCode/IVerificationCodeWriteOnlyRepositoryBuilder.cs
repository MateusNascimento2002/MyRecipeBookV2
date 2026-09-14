using Moq;
using MyRecipeBook.Domain.Interfaces.Repositories.VerificationCode;

namespace CommonTestUtilities.Repositories.VerificationCode;

public class IVerificationCodeWriteOnlyRepositoryBuilder
{
    public static IVerificationCodeWriteOnlyRepository Build()
    {
        var mock = new Mock<IVerificationCodeWriteOnlyRepository>();

        return mock.Object;
    }
}