namespace Domain.Results;

public record DomainError
{
    public static readonly DomainError None = new(string.Empty, string.Empty, DomainErrorType.Failure);
    public static readonly DomainError NullValue = new("Error.NullValue", "Null value was provided", DomainErrorType.Failure);

    private DomainError(string code, string description, DomainErrorType domainErrorType)
    {
        Code = code;
        Description = description;
        DomainErrorType = domainErrorType;
    }

    public string Code { get; }

    public string Description { get; }

    public DomainErrorType DomainErrorType { get; }

    public static DomainError Failure(string code, string description) => new(code, description, DomainErrorType.Failure);

    public static DomainError Validation(string code, string description) => new(code, description, DomainErrorType.Validation);

    public static DomainError NotFound(string code, string description) => new(code, description, DomainErrorType.NotFound);

    public static DomainError Conflict(string code, string description) => new(code, description, DomainErrorType.Conflict);
}
