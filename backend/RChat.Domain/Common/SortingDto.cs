namespace RChat.Domain.Common;

public class SortingDto <T>
    where T : Enum
{
    public SortingDirection Direction { get; init; }

    public T Column { get; init; } 
}