using RChat.Application.Abstractions.Messaging;
using RChat.Application.Users.CommonDtos;
using RChat.Domain.Users;

namespace RChat.Application.Users.GetById;

public record GetUserByIdQuery : IQuery<UserDto>
{
    public required int Id { get; init; }
}