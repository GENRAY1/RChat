namespace RChat.Application.Exceptions;

public class UnauthenticatedException()
    : ApiException(401, "Account is not authenticated");

