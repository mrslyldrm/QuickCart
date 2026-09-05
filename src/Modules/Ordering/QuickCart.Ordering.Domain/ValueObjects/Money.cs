using QuickCart.Ordering.Domain.Abstractions;

namespace QuickCart.Ordering.Domain.ValueObjects;

public sealed record Money
{
    public decimal Amount { get; }

    public string Currency { get; }

    public Money(decimal amount, string currency)
    {
        if (amount < 0)
        {
            throw new DomainException("Money amount cannot be negative.");
        }

        if (string.IsNullOrWhiteSpace(currency))
        {
            throw new DomainException("Currency is required.");
        }

        Amount = amount;
        Currency = currency.Trim().ToUpperInvariant();
    }

    public static Money Zero(string currency)
    {
        return new Money(0, currency);
    }

    public Money Add(Money other)
    {
        ArgumentNullException.ThrowIfNull(other);

        if (!string.Equals(
                Currency,
                other.Currency,
                StringComparison.OrdinalIgnoreCase))
        {
            throw new DomainException(
                $"Cannot add different currencies: {Currency} and {other.Currency}.");
        }

        return new Money(Amount + other.Amount, Currency);
    }

    public Money Multiply(int quantity)
    {
        if (quantity <= 0)
        {
            throw new DomainException("Quantity must be greater than zero.");
        }

        return new Money(Amount * quantity, Currency);
    }
}