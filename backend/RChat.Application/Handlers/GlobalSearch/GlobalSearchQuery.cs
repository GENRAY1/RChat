using RChat.Application.Abstractions.Messaging;
using RChat.Application.Dtos.GlobalSearch;

namespace RChat.Application.Handlers.GlobalSearch;

public record GlobalSearchQuery(string Query, int Take) : IQuery<GlobalSearchDtoResponse>;