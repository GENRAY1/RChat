namespace RChat.Application.Services.Search.User;

public interface IUserSearchService
{
    Task<bool> IndexAsync(int id, UserDocument userDocument, CancellationToken cancellationToken);
    Task<bool> UpdateAsync(int id, UpdateUserDocument userDocument, CancellationToken cancellationToken);
}