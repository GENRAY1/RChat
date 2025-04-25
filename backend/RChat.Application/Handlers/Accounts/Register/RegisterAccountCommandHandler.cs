using RChat.Application.Abstractions.Messaging;
using RChat.Application.Exceptions;
using RChat.Application.Services.Authentication;
using RChat.Domain.Accounts;
using RChat.Domain.Accounts.Repository;

namespace RChat.Application.Handlers.Accounts.Register;

public class RegisterAccountCommandHandler(
    IAccountRepository accountRepository,
    IPasswordEncryptorService passwordEncryptorService
    ) : ICommandHandler<RegisterAccountCommand, int>
{
    public async Task<int> Handle(RegisterAccountCommand request, CancellationToken cancellationToken)
    {
        Account? existingAccount = await accountRepository.GetAsync(
            new GetAccountParameters { Login = request.Login });
        
        if(existingAccount is not null)
            throw new ConflictDataException(nameof(existingAccount.Login));
        
        string passwordHash = passwordEncryptorService.Generate(request.Password);

        Account user = new Account
        {
            Login = request.Login,
            Password = passwordHash,
            RoleId = AccountRole.User.Id,
            CreatedAt = DateTime.UtcNow
        };
        
        int userId = await accountRepository.CreateAsync(user);
            
        return userId;
    }
}