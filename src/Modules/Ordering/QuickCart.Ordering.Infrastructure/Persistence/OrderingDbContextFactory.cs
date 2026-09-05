using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace QuickCart.Ordering.Infrastructure.Persistence;

public sealed class OrderingDbContextFactory
    : IDesignTimeDbContextFactory<OrderingDbContext>
{
    public OrderingDbContext CreateDbContext(
        string[] args)
    {
        string connectionString =
            Environment.GetEnvironmentVariable(
                "QUICKCART_DB")
            ??
            "Host=localhost;Port=5432;Database=quickcart;Username=postgres;Password=postgres";

        var optionsBuilder =
            new DbContextOptionsBuilder<OrderingDbContext>();

        optionsBuilder.UseNpgsql(
            connectionString,
            npgsqlOptions =>
            {
                npgsqlOptions.MigrationsHistoryTable(
                    "__EFMigrationsHistory",
                    "ordering");
            });

        return new OrderingDbContext(
            optionsBuilder.Options);
    }
}