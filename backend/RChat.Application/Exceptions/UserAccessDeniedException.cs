namespace RChat.Application.Exceptions;

public class UserAccessDeniedException(int userId, string entityType, int entityId) 
    : ApiException(403, $"User {userId} does not have access to {entityType} with identifier {entityId}");