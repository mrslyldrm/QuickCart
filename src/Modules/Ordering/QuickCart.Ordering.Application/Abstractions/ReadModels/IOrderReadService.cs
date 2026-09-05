using QuickCart.Ordering.Application.Orders.GetOrder;

namespace QuickCart.Ordering.Application.Abstractions.ReadModels;

public interface IOrderReadService
{
    Task<OrderDetails?> GetByIdAsync(
        Guid orderId,
        CancellationToken cancellationToken = default);
}