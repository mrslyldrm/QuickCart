using Microsoft.EntityFrameworkCore;
using QuickCart.Ordering.Application.Abstractions.ReadModels;
using QuickCart.Ordering.Application.Orders.GetOrder;

namespace QuickCart.Ordering.Infrastructure.Persistence.ReadModels;

internal sealed class OrderReadService
    : IOrderReadService
{
    private readonly OrderingDbContext _dbContext;

    public OrderReadService(OrderingDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<OrderDetails?> GetByIdAsync(Guid orderId, CancellationToken cancellationToken = default)
    {
        var result = await _dbContext.Orders
            .AsNoTracking()
            .Where(x => x.Id == orderId)
            .Select(x => new
            {
                x.Id,
                x.CustomerId,
                x.Status,
                x.CreatedAtUtc,
                x.PlacedAtUtc,

                Items = x.Items
                    .Select(item => new
                    {
                        item.ProductId,
                        item.ProductName,
                        UnitPrice =
                            item.UnitPrice.Amount,
                        item.UnitPrice.Currency,
                        item.Quantity
                    })
                    .ToList()
            })
            .SingleOrDefaultAsync(
                cancellationToken);

        if (result is null)
        {
            return null;
        }

        OrderItemDetails[] items = result.Items
            .Select(item =>
                new OrderItemDetails(
                    item.ProductId,
                    item.ProductName,
                    item.UnitPrice,
                    item.Currency,
                    item.Quantity,
                    item.UnitPrice * item.Quantity))
            .ToArray();

        string currency =
            items.FirstOrDefault()?.Currency ?? "TRY";

        decimal total =
            items.Sum(x => x.LineTotal);

        return new OrderDetails(
            result.Id,
            result.CustomerId,
            result.Status.ToString(),
            total,
            currency,
            result.CreatedAtUtc,
            result.PlacedAtUtc,
            items);
    }
}