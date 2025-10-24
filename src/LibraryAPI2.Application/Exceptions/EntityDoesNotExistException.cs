namespace LibraryAPI2.Application.Exceptions;

public class EntityDoesNotExistException : Exception
{
    public string Entity;
    public int RequestedId;

    public EntityDoesNotExistException(string entity, int requestedId)
        : base($"Entity '{entity}' with ID {requestedId} does not exist.")
    {
        Entity = entity;
        RequestedId = requestedId;
    }

    public EntityDoesNotExistException(string entity, int requestedId, string message)
        : base(message)
    {
        Entity = entity;
        RequestedId = requestedId;
    }
}