using RChat.Application.Abstractions.Messaging;
using RChat.Application.Exceptions;
using RChat.Domain.Users;
using RChat.Domain.Users.Repository;

namespace RChat.Application.Users.Update;

public class UpdateUserCommandHandler(IUserRepository userRepository)
    : ICommandHandler<UpdateUserCommand>
{
    public async Task Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        User? user = await userRepository.GetAsync(new GetUserParameters { Id = request.Id });
        
        if (user is null)
            throw new EntityNotFoundException(nameof(User), request.Id);

        user.Username = request.Username;
        user.Description = request.Description;
        user.DateOfBirth = request.DateOfBirth;
        user.UpdatedAt = DateTime.UtcNow;
        
        await userRepository.UpdateAsync(user);
    }
}