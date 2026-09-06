using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using QuickCart.Catalog.Infrastructure.Persistence;

namespace QuickCart.Catalog.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddCatalogInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        string connectionString =
            configuration.GetConnectionString("QuickCart")
            ?? throw new InvalidOperationException(
                "Connection string 'QuickCart' was not found.");

        services.AddDbContext<CatalogDbContext>(
            options =>
            {
                options.UseNpgsql(
                    connectionString,
                    npgsqlOptions =>
                    {
                        npgsqlOptions.MigrationsHistoryTable(
                            "__EFMigrationsHistory",
                            "catalog");
                    });
            });

        return services;
    }
}