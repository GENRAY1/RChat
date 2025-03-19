namespace RChat.Domain.Common;

public class MultiSortingDto <T> 
    where T : Enum
{
    public required SortingDirection Direction { get; init; }

    public required T[] Columns { get; init; }
}