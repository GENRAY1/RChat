namespace RChat.Application.Users.CommonDtos;

public record RoleDto
{
    public required int Id { get; init; }
    public required string Name { get; init; }
    public string? Description { get; init; }
}