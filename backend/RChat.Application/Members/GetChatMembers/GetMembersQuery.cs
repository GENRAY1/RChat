using RChat.Application.Abstractions.Messaging;
using RChat.Application.Members.Dtos;
using RChat.Domain.Common;
using RChat.Domain.Members;

namespace RChat.Application.Members.GetChatMembers;

public class GetMembersQuery : IQuery<List<MemberDto>>
{
    public required int ChatId { get; init; }
    public SortingDto<MemberSortingColumn>? Sorting { get; init; }
    public PaginationDto? Pagination { get; init; }
}