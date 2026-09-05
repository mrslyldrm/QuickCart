using QuickCart.Ordering.Domain.Orders;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuickCart.Ordering.Application.Abstractions.Persistence
{
    public interface IOrderRepository
    {
        Task<Order?> GetByIdAsync(Guid orderId, CancellationToken cancellationToken = default);
        Task AddAsync(Order order, CancellationToken cancellationToken = default);
    }
}
