namespace Lunches.Api.Domain.Events;

public record ItemsAddedToLunch(List<LunchItem> Items);