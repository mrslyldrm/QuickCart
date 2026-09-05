using QuickCart.Ordering.Domain.Abstractions;
using QuickCart.Ordering.Domain.ValueObjects;

namespace QuickCart.Ordering.Domain.Orders;

public sealed class OrderItem
{
    private OrderItem()
    {
    }

    internal OrderItem(
        Guid id,
        Guid productId,
        string productName,
        Money unitPrice,
        int quantity)
    {
        if (id == Guid.Empty)
        {
            throw new DomainException("Order item id is required.");
        }

        if (productId == Guid.Empty)
        {
            throw new DomainException("Product id is required.");
        }

        if (string.IsNullOrWhiteSpace(productName))
        {
            throw new DomainException("Product name is required.");
        }

        if (quantity <= 0)
        {
            throw new DomainException(
                "Order item quantity must be greater than zero.");
        }

        Id = id;
        ProductId = productId;
        ProductName = productName.Trim();
        UnitPrice = unitPrice
            ?? throw new ArgumentNullException(nameof(unitPrice));

        Quantity = quantity;
    }

    public Guid Id { get; private set; }

    public Guid ProductId { get; private set; }

    public string ProductName { get; private set; } = null!;

    public Money UnitPrice { get; private set; } = null!;

    public int Quantity { get; private set; }

    public Money LineTotal => UnitPrice.Multiply(Quantity);

    internal void IncreaseQuantity(int quantity)
    {
        if (quantity <= 0)
        {
            throw new DomainException(
                "Quantity to add must be greater than zero.");
        }

        Quantity += quantity;
    }
}