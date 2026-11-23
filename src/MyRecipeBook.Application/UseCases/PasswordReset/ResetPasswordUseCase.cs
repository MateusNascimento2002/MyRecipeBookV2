using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Domain.Interfaces.Repositories.UnitOfWork;
using MyRecipeBook.Domain.Interfaces.Repositories.User;
using MyRecipeBook.Domain.Interfaces.Repositories.VerificationCode;
using MyRecipeBook.Domain.Security.PasswordHashing;
using MyRecipeBook.Exception;
using MyRecipeBook.Exception.ExceptionBase;

namespace MyRecipeBook.Application.UseCases.PasswordReset;

public class ResetPasswordUseCase : IResetPasswordUseCase
{
    private const int ExpirationTimeInMinutes = 10;
    private readonly IUserReadOnlyRepository _userReadOnlyRepository;
    private readonly IVerificationCodeWriteOnlyRepository _verificationCodeWriteOnlyRepository;
    private readonly IVerificationCodeReadOnlyRepository _verificationCodeReadOnlyRepository;
    private readonly IUserUpdateOnlyRepository _userUpdateOnlyRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUnitOfWork _unitOfWork;

    public ResetPasswordUseCase(IUserReadOnlyRepository userReadOnlyRepository, 
        IVerificationCodeWriteOnlyRepository verificationCodeWriteOnlyRepository,
        IVerificationCodeReadOnlyRepository verificationCodeReadOnlyRepository,
        IUserUpdateOnlyRepository userUpdateOnlyRepository,
        IPasswordHasher passwordHasher,
        IUnitOfWork unitOfWork)
    {
        _userReadOnlyRepository = userReadOnlyRepository;
        _verificationCodeWriteOnlyRepository = verificationCodeWriteOnlyRepository;
        _verificationCodeReadOnlyRepository = verificationCodeReadOnlyRepository;
        _userUpdateOnlyRepository = userUpdateOnlyRepository;
        _passwordHasher = passwordHasher;
        _unitOfWork = unitOfWork;
    }
    
    public async Task Execute(RequestResetPasswordJson request)
    {
        var user = await _userReadOnlyRepository.GetByEmail(request.Email);
        if (user is null)
            throw new ErrorOnValidationException([ResourceMessagesException.VERIFICATION_CODE_INVALID]);
        
        var verificationCode = await _verificationCodeReadOnlyRepository.Get(user.Id, request.Code, Domain.Enums.VerificationCode.VerificationCodeType.PasswordRecovery);
        if(verificationCode is null)
            throw new ErrorOnValidationException([ResourceMessagesException.VERIFICATION_CODE_INVALID]);

        ValidateAndThrowOnFailures(request, verificationCode);
        
        var hashedPassword = _passwordHasher.HashPassword(request.NewPassword);
        
        await _userUpdateOnlyRepository.UpdatePassword(verificationCode.UserId, hashedPassword);
        
        await _unitOfWork.CommitAsync();
        
        await _verificationCodeWriteOnlyRepository.Delete(verificationCode);
    }
    
    private void ValidateAndThrowOnFailures(RequestResetPasswordJson request, Domain.Entities.VerificationCode verificationCode)
    {   
        var isCodeValid = verificationCode.CreatedAt.AddMinutes(ExpirationTimeInMinutes) >= DateTime.UtcNow;
        if (!isCodeValid)
            throw new ErrorOnValidationException([ResourceMessagesException.VERIFICATION_CODE_INVALID]);
        
        var result = new ResetPasswordValidator().Validate(request);
        
        if (!result.IsValid)
            throw new ErrorOnValidationException(result.Errors.Select(e => e.ErrorMessage).ToList());
    }
}