using Microsoft.EntityFrameworkCore;
using QuickCart.Ordering.Application.Abstractions.Persistence;
using QuickCart.Ordering.Domain.Orders;

namespace QuickCart.Ordering.Infrastructure.Persistence.Repositories;

internal sealed class OrderRepository : IOrderRepository
{
    private readonly OrderingDbContext _dbContext;

    public OrderRepository(OrderingDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Order?> GetByIdAsync(Guid orderId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Orders
            .Include(x => x.Items)
            .SingleOrDefaultAsync(
                x => x.Id == orderId,
                cancellationToken);
    }

    public async Task AddAsync(Order order, CancellationToken cancellationToken = default)
    {
        await _dbContext.Orders.AddAsync(order, cancellationToken);
    }
}