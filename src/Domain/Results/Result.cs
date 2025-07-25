namespace Domain.Results;

public class Result<T> : Result where T : notnull
{
    public T? Value { get; }

    protected internal Result(T? value, bool isSuccess, DomainError error) : base(isSuccess, error)
    {
        if (isSuccess && value is null)
        {
            throw new ArgumentException("Success result cannot have null value", nameof(value));
        }
        Value = value;
    }

    private T GetValue()
    {
        if (!IsSuccess)
        {
            throw new InvalidOperationException("Cannot get value from a failed result");
        }
        
        return Value!;
    }
    
    public static implicit operator T(Result<T> result)
    {
        return result.GetValue();
    }
}

public class Result
{
    protected Result(bool isSuccess, DomainError error)
    {
        if (isSuccess && error != DomainError.None || !isSuccess && error == DomainError.None)
        {
            throw new ArgumentException("Invalid error", nameof(error));
        }

        IsSuccess = isSuccess;
        Error = error;
    }

    public bool IsSuccess { get; }

    public bool IsFailure => !IsSuccess;

    public DomainError Error { get; }

    public static Result Success() => new(true, DomainError.None);

    public static Result<T> Success<T>(T value) where T : notnull 
        => new(value ?? throw new ArgumentNullException(nameof(value)), true, DomainError.None);

    public static Result Failure(DomainError domainError) => new(false, domainError);

    public static Result<T> Failure<T>(DomainError domainError) where T : notnull 
        => new(default, false, domainError);
}