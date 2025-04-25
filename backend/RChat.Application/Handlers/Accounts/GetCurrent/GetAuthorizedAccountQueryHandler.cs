using RChat.Application.Abstractions.Messaging;
using RChat.Application.Dtos.Accounts;
using RChat.Application.Exceptions;
using RChat.Application.Extensions.Accounts;
using RChat.Application.Services.Authentication;
using RChat.Domain.Accounts;
using RChat.Domain.Accounts.Repository;

namespace RChat.Application.Handlers.Accounts.GetCurrent;

public class GetAuthorizedAccountQueryHandler(
    IAccountRepository accountRepository,
    IAuthContext authContext
    ) : IQueryHandler<GetCurrentAccountQuery, AccountDto>
{
    public async Task<AccountDto> Handle(GetCurrentAccountQuery request, CancellationToken cancellationToken)
    {
        Account? account =
            await accountRepository.GetAsync(new GetAccountParameters{Id = authContext.AccountId});
        
        if(account is null) 
            throw new EntityNotFoundException(nameof(Account), authContext.AccountId);
        
        return account.MappingToDto();
    }
}