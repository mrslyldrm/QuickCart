using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using QuickCart.Ordering.Application.Abstractions.Persistence;
using QuickCart.Ordering.Application.Abstractions.ReadModels;
using QuickCart.Ordering.Infrastructure.Persistence;
using QuickCart.Ordering.Infrastructure.Persistence.ReadModels;
using QuickCart.Ordering.Infrastructure.Persistence.Repositories;

namespace QuickCart.Ordering.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddOrderingInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        string connectionString = configuration.GetConnectionString("QuickCart") ?? throw new InvalidOperationException("Connection string 'QuickCart' was not found.");

        services.AddDbContext<OrderingDbContext>(
            options =>
            {
                options.UseNpgsql(
                    connectionString,
                    npgsqlOptions =>
                    {
                        npgsqlOptions.MigrationsHistoryTable(
                            "__EFMigrationsHistory",
                            "ordering");
                    });
            });

        services.AddScoped<IOrderRepository, OrderRepository>();

        services.AddScoped<IUnitOfWork>(serviceProvider => serviceProvider.GetRequiredService<OrderingDbContext>());

        services.AddScoped<IOrderReadService, OrderReadService>();

        return services;
    }
}