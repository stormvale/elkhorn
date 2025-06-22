namespace Domain.Exceptions;

public class UnexpectedOperationException : Exception
{
    public UnexpectedOperationException()
    {
    }

    public UnexpectedOperationException(string message) : base(message)
    {
    }

    public UnexpectedOperationException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
