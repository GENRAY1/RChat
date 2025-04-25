using RChat.Application.Abstractions.Messaging;
using RChat.Application.Common;
using RChat.Application.Common.Sorting;
using RChat.Application.Dtos.Members;
using RChat.Domain.Members;

namespace RChat.Application.Handlers.Members.GetChatMembers;

public class GetMembersQuery : IQuery<List<MemberDto>>
{
    public required int ChatId { get; init; }
    public int[]? UserIds { get; init; }
     public SortingDto<MemberSortingColumn>? Sorting { get; init; }
    public PaginationDto? Pagination { get; init; }
}