using RChat.Application.Abstractions.Messaging;

namespace RChat.Application.GlobalSearch;

public record GlobalSearchQuery(string Query, int Take) : IQuery<GlobalSearchDtoResponse>;