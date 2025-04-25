using RChat.Application.Abstractions.Messaging;
using RChat.Application.Dtos.Users;

namespace RChat.Application.Handlers.Users.GetById;

public record GetUserByIdQuery : IQuery<UserDto>
{
    public required int Id { get; init; }
}