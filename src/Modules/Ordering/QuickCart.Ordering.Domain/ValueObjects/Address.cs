using QuickCart.Ordering.Domain.Abstractions;

namespace QuickCart.Ordering.Domain.ValueObjects;

public sealed record Address
{
    public string City { get; }
    public string District { get; }
    public string Line { get; }
    public string? PostalCode { get; }

    public Address(
        string city,
        string district,
        string line,
        string? postalCode = null)
    {
        if (string.IsNullOrWhiteSpace(city))
        {
            throw new DomainException("City is required.");
        }

        if (string.IsNullOrWhiteSpace(district))
        {
            throw new DomainException("District is required.");
        }

        if (string.IsNullOrWhiteSpace(line))
        {
            throw new DomainException("Address line is required.");
        }

        City = city.Trim();
        District = district.Trim();
        Line = line.Trim();
        PostalCode = postalCode?.Trim();
    }
}