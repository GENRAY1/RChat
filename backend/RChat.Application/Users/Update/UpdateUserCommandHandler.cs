using RChat.Application.Abstractions.Messaging;
using RChat.Application.Abstractions.Services.Authentication;
using RChat.Application.Exceptions;
using RChat.Domain.Accounts;
using RChat.Domain.Users;
using RChat.Domain.Users.Repository;

namespace RChat.Application.Users.Update;

public class UpdateUserCommandHandler(
    IUserRepository userRepository,
    IAuthContext authContext
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

        user.Username = request.Username;
        user.Description = request.Description;
        user.DateOfBirth = request.DateOfBirth;
        user.UpdatedAt = DateTime.UtcNow;
        user.Firstname = request.Firstname;
        user.Lastname = request.Lastname;
        
        await userRepository.UpdateAsync(user);
    }
}