namespace LibraryAPI.Exceptions;

public class RepositoryUpdateException : Exception
{
    public RepositoryUpdateException() { }

    public RepositoryUpdateException(string message) : base(message) { }

    public RepositoryUpdateException(string message, Exception innerException) : base(message, innerException) { }
}