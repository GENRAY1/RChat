namespace RChat.Application.Exceptions;

public class ValidationException(string message)
    : ApiException(400, message);
