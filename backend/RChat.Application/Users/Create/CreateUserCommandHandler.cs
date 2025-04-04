using RChat.Application.Abstractions.Messaging;
using RChat.Application.Abstractions.Services.Authentication;
using RChat.Application.Exceptions;
using RChat.Application.Users.CommonDtos;
using RChat.Application.Users.Extensions;
using RChat.Domain.Accounts;
using RChat.Domain.Accounts.Repository;
using RChat.Domain.Users;
using RChat.Domain.Users.Repository;

namespace RChat.Application.Users.Create;

public class CreateUserCommandHandler(
    IAuthContext authContext,
    IAccountRepository accountRepository,
    IUserRepository userRepository
) : ICommandHandler<CreateUserCommand, UserDto>
{
    public async Task<UserDto> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    { 
        Account currentAccount =
            (await accountRepository.GetAsync(new GetAccountParameters { Id = authContext.AccountId }))!;

        if (currentAccount.User is not null)
            throw new ConflictDataException(nameof(User));
        
        User user = new User
        {
            AccountId = authContext.AccountId,
            Firstname = request.Firstname,
            Lastname = request.Lastname,
            Username = request.Username,
            DateOfBirth = request.DateOfBirth,
            Description = request.Description,
            CreatedAt = DateTime.UtcNow
        };

        int id = await userRepository.CreateAsync(user);

        user.Id = id;

        return user.MappingToDto();
    }
}