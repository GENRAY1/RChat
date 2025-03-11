namespace RChat.Application.Exceptions;

public class EntityNotFoundException(string entityName, int entityId) 
    : ApiException(404, $"The entity '{entityName}' with identifier '{entityId}' was not found");