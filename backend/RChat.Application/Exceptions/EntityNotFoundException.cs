namespace RChat.Application.Exceptions;

public class EntityNotFoundException : ApiException
{
    public EntityNotFoundException(string entityName, int entityId)
        : base(404, $"The entity '{entityName}' with identifier '{entityId}' was not found") { }
    
    public EntityNotFoundException(string entityName) 
        : base(404, $"The entity '{entityName}' was not found") { }
}