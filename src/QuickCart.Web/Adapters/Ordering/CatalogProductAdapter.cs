using Microsoft.EntityFrameworkCore;
using QuickCart.Catalog.Infrastructure.Persistence;
using QuickCart.Ordering.Application.Abstractions.Catalog;

namespace QuickCart.Web.Adapters.Ordering;

internal sealed class CatalogProductAdapter : IProductCatalog
{
    private readonly CatalogDbContext _dbContext;

    public CatalogProductAdapter(CatalogDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyCollection<ProductSnapshot>> GetProductsAsync(IReadOnlyCollection<Guid> productIds, CancellationToken cancellationToken = default)
    {
        if (productIds.Count == 0)
        {
            return [];
        }

        return await _dbContext.Products
            .AsNoTracking()
            .Where(product =>
                productIds.Contains(product.Id))
            .Select(product =>
                new ProductSnapshot(
                    product.Id,
                    product.Name,
                    product.Price,
                    product.Currency,
                    product.IsActive))
            .ToArrayAsync(cancellationToken);
    }
}