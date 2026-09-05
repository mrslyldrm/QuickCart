using MediatR;

namespace QuickCart.Ordering.Application.Orders.PlaceOrder;

public sealed record PlaceOrderCommand(
    Guid CustomerId,
    PlaceOrderAddress Address,
    IReadOnlyCollection<PlaceOrderItem> Items)
    : IRequest<Guid>;

public sealed record PlaceOrderAddress(
    string City,
    string District,
    string Line,
    string? PostalCode);

public sealed record PlaceOrderItem(
    Guid ProductId,
    int Quantity);