using RChat.Application.Abstractions.Messaging;
using RChat.Application.Users.CommonDtos;
using RChat.Application.Users.Extensions;
using RChat.Domain.Users;
using RChat.Domain.Users.Repository;

namespace RChat.Application.Users.GetList;

public class GetUsersQueryHandler(IUserRepository userRepository)
    : IQueryHandler<GetUsersQuery, List<UserDto>>
{
    public async Task<List<UserDto>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        List<User> users = await userRepository.GetListAsync(
            new GetUserListParameters
            {
                Sorting = request.Sorting,
                Pagination = request.Pagination
            });
        
        return users
            .Select(user => user.MappingToDto())
            .ToList();
    }
}