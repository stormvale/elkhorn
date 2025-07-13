namespace Domain.Results;

public class Result<T> : Result
{
    public T? Value { get; set; }

    protected internal Result(T? value, bool isSuccess, DomainError error) : base(isSuccess, error)
    {
        Value = value;
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

    public static Result<T> Success<T>(T value) => new(value, true, DomainError.None);

    public static Result Failure(DomainError domainError) => new(false, domainError);

    public static Result<T> Failure<T>(DomainError domainError) => new(default, false, domainError);
}