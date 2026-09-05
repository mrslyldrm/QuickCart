using QuickCart.Ordering.Domain.Abstractions;
using QuickCart.Ordering.Domain.Orders.Events;
using QuickCart.Ordering.Domain.ValueObjects;

namespace QuickCart.Ordering.Domain.Orders;

public sealed class Order : AggregateRoot
{
    private readonly List<OrderItem> _items = [];

    private Order()
    {
    }

    private Order(
        Guid id,
        Guid customerId,
        Address shippingAddress,
        DateTimeOffset createdAtUtc)
    {
        if (id == Guid.Empty)
        {
            throw new DomainException("Order id is required.");
        }

        if (customerId == Guid.Empty)
        {
            throw new DomainException("Customer id is required.");
        }

        Id = id;
        CustomerId = customerId;
        ShippingAddress = shippingAddress
            ?? throw new ArgumentNullException(nameof(shippingAddress));

        CreatedAtUtc = createdAtUtc;
        Status = OrderStatus.Draft;
    }

    public Guid Id { get; private set; }

    public Guid CustomerId { get; private set; }

    public Address ShippingAddress { get; private set; } = null!;

    public OrderStatus Status { get; private set; }

    public DateTimeOffset CreatedAtUtc { get; private set; }

    public DateTimeOffset? PlacedAtUtc { get; private set; }

    public IReadOnlyCollection<OrderItem> Items => _items;

    public Money Total
    {
        get
        {
            if (_items.Count == 0)
            {
                return Money.Zero("TRY");
            }

            Money total = Money.Zero(_items[0].UnitPrice.Currency);

            foreach (OrderItem item in _items)
            {
                total = total.Add(item.LineTotal);
            }

            return total;
        }
    }

    public static Order Create(
        Guid id,
        Guid customerId,
        Address shippingAddress,
        DateTimeOffset createdAtUtc)
    {
        return new Order(
            id,
            customerId,
            shippingAddress,
            createdAtUtc);
    }

    public void AddItem(
        Guid productId,
        string productName,
        Money unitPrice,
        int quantity)
    {
        EnsureDraft();

        if (quantity <= 0)
        {
            throw new DomainException(
                "Quantity must be greater than zero.");
        }

        ArgumentNullException.ThrowIfNull(unitPrice);

        EnsureCurrencyIsCompatible(unitPrice);

        OrderItem? existingItem =
            _items.SingleOrDefault(x => x.ProductId == productId);

        if (existingItem is not null)
        {
            existingItem.IncreaseQuantity(quantity);
            return;
        }

        var item = new OrderItem(
            Guid.NewGuid(),
            productId,
            productName,
            unitPrice,
            quantity);

        _items.Add(item);
    }

    public void ChangeShippingAddress(Address shippingAddress)
    {
        EnsureDraft();

        ShippingAddress = shippingAddress
            ?? throw new ArgumentNullException(nameof(shippingAddress));
    }

    public void Place(DateTimeOffset placedAtUtc)
    {
        EnsureDraft();

        if (_items.Count == 0)
        {
            throw new DomainException(
                "An order must contain at least one item.");
        }

        Status = OrderStatus.PendingStockReservation;
        PlacedAtUtc = placedAtUtc;

        RaiseDomainEvent(
            new OrderPlacedDomainEvent(Id));
    }

    public void ConfirmStockReservation()
    {
        if (Status != OrderStatus.PendingStockReservation)
        {
            throw new DomainException(
                $"Order cannot be confirmed from status {Status}.");
        }

        Status = OrderStatus.Confirmed;
    }

    public void RejectStockReservation()
    {
        if (Status != OrderStatus.PendingStockReservation)
        {
            throw new DomainException(
                $"Stock reservation cannot be rejected from status {Status}.");
        }

        Status = OrderStatus.Rejected;
    }

    public void Complete()
    {
        if (Status != OrderStatus.Confirmed)
        {
            throw new DomainException(
                $"Order cannot be completed from status {Status}.");
        }

        Status = OrderStatus.Completed;
    }

    public void Cancel()
    {
        if (Status is OrderStatus.Completed
            or OrderStatus.Rejected
            or OrderStatus.Cancelled)
        {
            throw new DomainException(
                $"Order cannot be cancelled from status {Status}.");
        }

        Status = OrderStatus.Cancelled;
    }

    private void EnsureDraft()
    {
        if (Status != OrderStatus.Draft)
        {
            throw new DomainException(
                $"Order cannot be modified when status is {Status}.");
        }
    }

    private void EnsureCurrencyIsCompatible(Money price)
    {
        if (_items.Count == 0)
        {
            return;
        }

        string orderCurrency = _items[0].UnitPrice.Currency;

        if (!string.Equals(
                orderCurrency,
                price.Currency,
                StringComparison.OrdinalIgnoreCase))
        {
            throw new DomainException(
                "All order items must use the same currency.");
        }
    }
}