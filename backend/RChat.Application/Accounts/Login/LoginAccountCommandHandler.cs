using RChat.Application.Abstractions.Messaging;
using RChat.Application.Abstractions.Services.Authentication;
using RChat.Application.Exceptions;
using RChat.Domain.Accounts;
using RChat.Domain.Accounts.Repository;

namespace RChat.Application.Accounts.Login;

public class LoginAccountCommandHandler(
    IPasswordEncryptorService passwordEncryptor,
    IAccountRepository accountRepository,
    IJwtProvider jwtProvider
    ) : ICommandHandler<LoginAccountCommand, string>
{
    public async Task<string> Handle(LoginAccountCommand request, CancellationToken cancellationToken)
    {
        Account? account = await accountRepository
            .GetAsync(new GetAccountParameters{ Login = request.Login});
        
        if (account is null || !passwordEncryptor.Verify(request.Password, account.Password))
            throw new InvalidCredentialsException();

        string accessToken = jwtProvider.GenerateAccessToken(account.Id, account.AccountRole.Name);

        return accessToken;
    }
}