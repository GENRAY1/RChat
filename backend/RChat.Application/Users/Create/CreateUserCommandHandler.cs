using RChat.Application.Abstractions;
using RChat.Application.Abstractions.Messaging;
using RChat.Application.Exceptions;
using RChat.Application.Services.Authentication;
using RChat.Application.Services.Search.User;
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
    IUserRepository userRepository,
    IUserSearchService userSearchService,
    IBackgroundTaskQueue backgroundTaskQueue
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

        user.Id = await userRepository.CreateAsync(user);
        
        backgroundTaskQueue.Enqueue(async token =>
        {
            await userSearchService.IndexAsync(user.Id, new UserDocument
            {
                Firstname = user.Firstname,
                Lastname = user.Lastname,
                Username = user.Username
            }, token);
        });
        
        return user.MappingToDto();
    }
}