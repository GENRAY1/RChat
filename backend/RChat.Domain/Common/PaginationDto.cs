namespace RChat.Domain.Common;

public class PaginationDto
{
    public required int Skip { get; init; }
    public required int Take { get; init; }
}