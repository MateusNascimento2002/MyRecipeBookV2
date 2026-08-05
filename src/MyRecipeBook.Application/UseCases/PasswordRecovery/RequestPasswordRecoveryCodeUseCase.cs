using System.Security.Cryptography;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Domain.Enums.VerificationCode;
using MyRecipeBook.Domain.Interfaces.Repositories.UnitOfWork;
using MyRecipeBook.Domain.Interfaces.Repositories.User;
using MyRecipeBook.Domain.Interfaces.Repositories.VerificationCode;

namespace MyRecipeBook.Application.UseCases.PasswordRecovery;

public class RequestPasswordRecoveryCodeUseCase : IRequestPasswordRecoveryCodeUseCase
{
    private readonly IUserReadOnlyRepository  _userReadOnlyRepository;
    private readonly IVerificationCodeWriteOnlyRepository  _verificationCodeWriteOnlyRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RequestPasswordRecoveryCodeUseCase(IUserReadOnlyRepository userReadOnlyRepository, IVerificationCodeWriteOnlyRepository verificationCodeWriteOnlyRepository, IUnitOfWork unitOfWork)
    {
        _userReadOnlyRepository = userReadOnlyRepository;
        _verificationCodeWriteOnlyRepository = verificationCodeWriteOnlyRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task Execute(RequestPasswordRecoveryJson request)
    {
        var user = await _userReadOnlyRepository.GetByEmail(request.Email);
        if (user == null)
            return;

        var code = RandomNumberGenerator.GetInt32(1, 1_000_000);

        var verificationCode = new VerificationCode()
        {
            Code = code.ToString("D6"),
            Type = VerificationCodeType.PasswordRecovery,
            UserId = user.Id,
        };
        
        // TODO: Send e-mail
        
        await _verificationCodeWriteOnlyRepository.AddAsync(verificationCode);
        
        await _unitOfWork.CommitAsync();
    }
}