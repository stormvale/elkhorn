using Domain.Results;

namespace Cart.Api.DomainErrors;

public class CartErrors
{
    public static DomainError NotFound(Guid id)
        => DomainError.NotFound("Cart.NotFound", $"Cart with Id '{id}' was not found.");
}

