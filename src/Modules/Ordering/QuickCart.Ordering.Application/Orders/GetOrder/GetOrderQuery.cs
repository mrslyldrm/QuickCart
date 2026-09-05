using MediatR;

namespace QuickCart.Ordering.Application.Orders.GetOrder;

public sealed record GetOrderQuery(Guid OrderId)
    : IRequest<OrderDetails?>;