using RChat.Application.Abstractions.Messaging;
using RChat.Application.Abstractions.Services.Authentication;
using RChat.Application.Exceptions;
using RChat.Domain.Users;
using RChat.Domain.Users.Repository;

namespace RChat.Application.Users.Register;

public class RegisterUserCommandHandler(
    IUserRepository userRepository,
    IPasswordEncryptorService passwordEncryptorService
    ) : ICommandHandler<RegisterUserCommand, int>
{
    public async Task<int> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        User? existingUser = await userRepository.GetAsync(
            new GetUserParameters { Login = request.Login });
        
        if(existingUser is not null)
            throw new ConflictDataException(nameof(existingUser.Login));
        
        string passwordHash = passwordEncryptorService.Generate(request.Password);

        User user = new User
        {
            Login = request.Login,
            Password = passwordHash,
            RoleId = UserRole.User.Id
        };
        
        int userId = await userRepository.CreateAsync(user);
            
        return userId;
    }
}