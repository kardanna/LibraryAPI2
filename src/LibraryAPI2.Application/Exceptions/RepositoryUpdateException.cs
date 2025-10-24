namespace LibraryAPI2.Application.Exceptions;

public class RepositoryUpdateException : Exception
{
    public RepositoryOperation Operation;
    public string Entity;
    public override string Message
    {
        get
        {
            var operation = Operation switch
            {
                RepositoryOperation.Add => "add",
                RepositoryOperation.Delete => "delete",
                _ => "UNKNOWN OPERATION"
            };

            return $"Failed to perform {operation} operation on uderlying collection of '{Entity}' entity";
        }
    }

    public RepositoryUpdateException(string entity, RepositoryOperation operation)
    {
        Entity = entity.ToString();
        Operation = operation;
    }
}