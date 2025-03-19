using RChat.Application.Exceptions;
using RChat.Domain.Users;
using RChat.Domain.Users.Repository;

namespace RChat.Application.Users.Extensions;

public static class UserRepositoryExtensions
{
    public static async Task<User> GetByIdOrThrowAsync(
        this IUserRepository repository,
        int id)
    {
        User? user = await repository.GetAsync(
            new GetUserParameters
            {
                Id = id
            });
        
        if (user is null)
            throw new EntityNotFoundException(nameof(User), id);
        
        return user;
    }
}