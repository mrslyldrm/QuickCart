using QuickCart.Ordering.Domain.Abstractions;

namespace QuickCart.Ordering.Domain.Orders.Events;

public sealed record OrderPlacedDomainEvent(Guid OrderId) : IDomainEvent;