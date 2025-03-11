namespace RChat.Application.Exceptions;

public class ConflictDataException(string fieldName) 
    : ApiException(409, $"{fieldName} already exists");