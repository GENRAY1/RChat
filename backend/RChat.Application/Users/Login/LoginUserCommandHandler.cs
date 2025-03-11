using RChat.Application.Abstractions.Messaging;
using RChat.Application.Abstractions.Services.Authentication;
using RChat.Application.Exceptions;
using RChat.Application.Users.Extensions;
using RChat.Domain.Users;
using RChat.Domain.Users.Repository;

namespace RChat.Application.Users.Login;

public class LoginUserCommandHandler(
    IPasswordEncryptorService passwordEncryptor,
    IJwtProvider jwtProvider,
    IUserRepository userRepository
    ) : ICommandHandler<LoginUserCommand, string>
{
    public async Task<string> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        User? user = await userRepository.GetAsync(new GetUserParameters { Login = request.Login});
        
        if (user is null || !passwordEncryptor.Verify(request.Password, user.Password))
            throw new InvalidCredentialsException();

        string accessToken = jwtProvider.GenerateAccessToken(user.MappingToDto());

        return accessToken;
    }
}