namespace RChat.Application.Exceptions;

public class ConflictDataException(string name) 
    : ApiException(409, $"{name} already exists");