namespace QuickCart.Ordering.Application.Orders.PlaceOrder;

public sealed class ProductUnavailableException : Exception
{
    public ProductUnavailableException(Guid productId)
        : base($"Product '{productId}' is unavailable.")
    {
        ProductId = productId;
    }

    public Guid ProductId { get; }
}