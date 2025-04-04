namespace RChat.Application.Exceptions;

public class IncompleteUserRegistrationException()
    : ApiException(403, "Cannot proceed with the action. User registration is not complete");