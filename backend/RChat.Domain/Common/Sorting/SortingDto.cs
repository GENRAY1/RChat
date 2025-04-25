namespace RChat.Application.Common.Sorting;

public class SortingDto <T>
    where T : Enum
{
    public SortingDirection Direction { get; init; }

    public T Column { get; init; } 
}