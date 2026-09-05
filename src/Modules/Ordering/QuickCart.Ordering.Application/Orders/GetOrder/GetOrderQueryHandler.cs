using MediatR;
using QuickCart.Ordering.Application.Abstractions.ReadModels;

namespace QuickCart.Ordering.Application.Orders.GetOrder;

public sealed class GetOrderQueryHandler
    : IRequestHandler<GetOrderQuery, OrderDetails?>
{
    private readonly IOrderReadService _orderReadService;

    public GetOrderQueryHandler(
        IOrderReadService orderReadService)
    {
        _orderReadService = orderReadService;
    }

    public Task<OrderDetails?> Handle(
        GetOrderQuery request,
        CancellationToken cancellationToken)
    {
        return _orderReadService.GetByIdAsync(
            request.OrderId,
            cancellationToken);
    }
}