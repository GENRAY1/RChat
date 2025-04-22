using RChat.Application.Abstractions;
using RChat.Application.Abstractions.Messaging;
using RChat.Application.Exceptions;
using RChat.Application.Services.Authentication;
using RChat.Application.Services.Search.User;
using RChat.Domain.Accounts;
using RChat.Domain.Users;
using RChat.Domain.Users.Repository;

namespace RChat.Application.Users.Update;

public class UpdateUserCommandHandler(
    IUserRepository userRepository,
    IAuthContext authContext,
    IUserSearchService userSearchService,
    IBackgroundTaskQueue backgroundTaskQueue
    ) : ICommandHandler<UpdateUserCommand>
{
    public async Task Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        User? user = await userRepository.GetAsync(new GetUserParameters { Id = request.Id });
        
        if (user is null)
            throw new EntityNotFoundException(nameof(User), request.Id);

        if (authContext.Role == AccountRole.User.Name)
        {
            User accountUser = await authContext.GetUserAsync();
            
            if (accountUser.Id != user.Id)
                throw new UserAccessDeniedException(accountUser.Id, nameof(User), user.Id);
        }

        bool needUpdateDocument = NeedUpdateDocument(user, request);
        
        user.Username = request.Username;
        user.Description = request.Description;
        user.DateOfBirth = request.DateOfBirth;
        user.UpdatedAt = DateTime.UtcNow;
        user.Firstname = request.Firstname;
        user.Lastname = request.Lastname;
        
        await userRepository.UpdateAsync(user);

        if (needUpdateDocument)
        {
            backgroundTaskQueue.Enqueue(async token =>
            {
                await userSearchService.UpdateAsync(
                    user.Id,
                    new UpdateUserDocument
                    {
                        Username = user.Username,
                        Firstname = user.Firstname,
                        Lastname = user.Lastname
                    },
                    token);
            });
        }
    }

    private bool NeedUpdateDocument(User oldUser, UpdateUserCommand updateCommand)
    {
        if(oldUser.Firstname != updateCommand.Firstname) return true;
        
        if(oldUser.Lastname != updateCommand.Lastname) return true;
        
        if(oldUser.Username != updateCommand.Username) return true;
        
        return false;
    }
}