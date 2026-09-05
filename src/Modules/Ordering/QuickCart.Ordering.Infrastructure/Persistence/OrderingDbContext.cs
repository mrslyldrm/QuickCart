using Microsoft.EntityFrameworkCore;
using QuickCart.Ordering.Application.Abstractions.Persistence;
using QuickCart.Ordering.Domain.Orders;

namespace QuickCart.Ordering.Infrastructure.Persistence;

public sealed class OrderingDbContext
    : DbContext, IUnitOfWork
{
    public OrderingDbContext(
        DbContextOptions<OrderingDbContext> options)
        : base(options)
    {
    }

    public DbSet<Order> Orders =>
        Set<Order>();

    protected override void OnModelCreating(
        ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("ordering");

        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(OrderingDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}