namespace RChat.Application.Exceptions;

public class UnauthenticatedException()
    : ApiException(401, "User is not authenticated");

