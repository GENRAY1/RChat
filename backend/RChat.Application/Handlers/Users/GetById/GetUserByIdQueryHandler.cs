using RChat.Application.Abstractions.Messaging;
using RChat.Application.Dtos.Users;
using RChat.Application.Exceptions;
using RChat.Application.Extensions.Users;
using RChat.Domain.Users;
using RChat.Domain.Users.Repository;

namespace RChat.Application.Handlers.Users.GetById;

public class GetUserByIdQueryHandler(IUserRepository userRepository) 
    : IQueryHandler<GetUserByIdQuery, UserDto>
{
    public async Task<UserDto> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        User? user = await userRepository.GetAsync(new GetUserParameters { Id = request.Id });
        
        if (user is null)
            throw new EntityNotFoundException(nameof(User), request.Id);

        return user.MappingToDto();
    }
}