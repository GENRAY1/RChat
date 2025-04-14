using RChat.Application.Abstractions.Messaging;
using RChat.Application.Services.Search.GlobalSearch;
using RChat.Domain.Chats;
using RChat.Domain.Chats.Repository;

namespace RChat.Application.GlobalSearch;

public class GlobalSearchQueryHandler(
    IGlobalSearchService globalSearchService,
    IChatRepository chatRepository
) : IQueryHandler<GlobalSearchQuery, GlobalSearchDtoResponse>
{
    public async Task<GlobalSearchDtoResponse> Handle(GlobalSearchQuery request, CancellationToken cancellationToken)
    {
        var searchResult = await globalSearchService.Search(request.Query, request.Take, cancellationToken);

        List<ChatSearchDto> chatResponse = [];

        if (searchResult.Chats.Any())
        {
            var chatIds = searchResult.Chats
                .Select(x => x.Id)
                .ToArray();

            var chats = await chatRepository.GetListAsync(
                new GetChatListParameters
                {
                    ChatIds = chatIds
                });

            foreach (var c in searchResult.Chats)
            {
                Chat? chat = chats.FirstOrDefault(x => x.Id == c.Id);

                if (chat == null) continue;

                chatResponse.Add(new ChatSearchDto
                {
                    Id = c.Id,
                    Name = c.Document.Name,
                    Type = c.Document.Type,
                    MemberCount = chat.MemberCount
                });
            }
        }

        List<UserSearchDto> usersResponse = searchResult.Users.Select(
            u => new UserSearchDto
            {
                Id = u.Id,
                Username = u.Document.Username,
                Firstname = u.Document.Firstname,
                Lastname = u.Document.Lastname,
            }).ToList();

        return new GlobalSearchDtoResponse
        {
            Users = usersResponse,
            Chats = chatResponse
        };
    }
}