using QuickCart.Ordering.Domain.Abstractions;
using QuickCart.Ordering.Domain.Orders;
using QuickCart.Ordering.Domain.ValueObjects;

namespace QuickCart.Ordering.UnitTests.Orders;

public sealed class OrderTests
{
    [Fact]
    public void CannotAddZeroQuantity()
    {
        // Arrange
        Order order = CreateOrder();

        // Act
        Action action = () =>
            order.AddItem(
                Guid.NewGuid(),
                "Milk",
                new Money(45m, "TRY"),
                0);

        // Assert
        Assert.Throws<DomainException>(action);
    }

    private static Order CreateOrder()
    {
        return Order.Create(
            Guid.NewGuid(),
            Guid.NewGuid(),
            new Address(
                "Istanbul",
                "Sultanbeyli",
                "Test Street No:1"),
            DateTimeOffset.UtcNow);
    }

    [Fact]
    public void CompletedOrderCannotBeModified()
    {
        // Arrange
        Order order = CreateOrder();

        order.AddItem(
            Guid.NewGuid(),
            "Milk",
            new Money(45m, "TRY"),
            1);

        order.Place(DateTimeOffset.UtcNow);

        order.ConfirmStockReservation();

        order.Complete();

        // Act
        Action action = () =>
            order.AddItem(
                Guid.NewGuid(),
                "Bread",
                new Money(15m, "TRY"),
                1);

        // Assert
        Assert.Throws<DomainException>(action);
    }

    [Fact]
    public void CannotCompleteOrderBeforeStockReservationIsConfirmed()
    {
        // Arrange
        Order order = CreateOrder();

        order.AddItem(
            Guid.NewGuid(),
            "Milk",
            new Money(45m, "TRY"),
            1);

        order.Place(DateTimeOffset.UtcNow);

        // Act
        Action action = order.Complete;

        // Assert
        Assert.Throws<DomainException>(action);
    }
}