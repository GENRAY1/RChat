namespace RChat.Domain.Common;

public class Pagination
{
    public required int Skip { get; init; }
    public required int Take { get; init; }
}