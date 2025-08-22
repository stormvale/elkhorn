namespace Domain.Interfaces;

public interface IPurchasable
{
    /// <summary>
    /// The intention is for all IPurchasable items to be addable to a Cart. The current
    /// Cart implementation maintains a list of added items in an Items property of type
    /// IList&lt;ICartItem>, which the JSON serilaizer (System.Text.Json) cannot materialize
    /// without explicit polymorphic configuration. We could change this to instead use a
    /// concrete List&lt;CartItem> type instead, but I've chosen to go a different way for
    /// learning purposes.
    ///
    /// Instead, we will support polymorphic deserialization using this discriminator property
    /// (Type) to map JSON objects to their corresponding concrete types by creating a custom
    /// JsonConverter for ICartItem.
    ///
    /// UPDATE: going with the concrete CartItem implementation for now, so not needed.
    /// </summary>
    // public string Type { get; }
    
    public string Name { get; }
    
    public decimal Price { get; }
}