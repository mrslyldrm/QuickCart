namespace QuickCart.Catalog.Domain.Products;

public sealed class Product
{
    private Product()
    {
    }

    private Product(Guid id, string name, decimal price, string currency)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Product id cannot be empty.", nameof(id));
        }

        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Product name cannot be empty.", nameof(name));
        }

        if (price < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(price), "Product price cannot be negative.");
        }

        if (string.IsNullOrWhiteSpace(currency))
        {
            throw new ArgumentException("Currency cannot be empty.", nameof(currency));
        }

        Id = id;
        Name = name.Trim();
        Price = price;
        Currency = currency.Trim().ToUpperInvariant();
        IsActive = true;
    }

    public Guid Id { get; private set; }

    public string Name { get; private set; } = null!;

    public decimal Price { get; private set; }

    public string Currency { get; private set; } = null!;

    public bool IsActive { get; private set; }

    public static Product Create(Guid id, string name, decimal price, string currency)
    {
        return new Product(
            id,
            name,
            price,
            currency);
    }

    public void ChangePrice(decimal price)
    {
        if (price < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(price), "Product price cannot be negative.");
        }

        Price = price;
    }

    public void Activate()
    {
        IsActive = true;
    }

    public void Deactivate()
    {
        IsActive = false;
    }
}