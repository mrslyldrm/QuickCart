namespace QuickCart.Ordering.Application.Orders.GetOrder;

public sealed record OrderDetails(
    Guid Id,
    Guid CustomerId,
    string Status,
    decimal Total,
    string Currency,
    DateTimeOffset CreatedAtUtc,
    DateTimeOffset? PlacedAtUtc,
    IReadOnlyCollection<OrderItemDetails> Items);

public sealed record OrderItemDetails(
    Guid ProductId,
    string ProductName,
    decimal UnitPrice,
    string Currency,
    int Quantity,
    decimal LineTotal);