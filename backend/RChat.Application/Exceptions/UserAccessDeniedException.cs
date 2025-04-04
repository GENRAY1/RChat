namespace RChat.Application.Exceptions;

public class UserAccessDeniedException(int userId, string entityType, int entityId) 
    : ApiException(403, $"User {userId} does not have permission to perform this operation on the {entityType} with identifier {entityId}");