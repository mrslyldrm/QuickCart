namespace QuickCart.Ordering.Application.Abstractions.Catalog;

public interface IProductCatalog
{
    Task<IReadOnlyCollection<ProductSnapshot>> GetProductsAsync(IReadOnlyCollection<Guid> productIds, CancellationToken cancellationToken = default);
}

public sealed record ProductSnapshot(
    Guid Id,
    string Name,
    decimal Price,
    string Currency,
    bool IsActive);