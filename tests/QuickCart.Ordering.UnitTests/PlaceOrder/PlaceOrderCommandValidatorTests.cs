using QuickCart.Ordering.Application.Orders.PlaceOrder;

namespace QuickCart.Ordering.UnitTests.Orders.PlaceOrder;

public sealed class PlaceOrderCommandValidatorTests
{
    [Fact]
    public async Task QuantityMustBeGreaterThanZero()
    {
        // Arrange
        var validator = new PlaceOrderCommandValidator();

        var command = new PlaceOrderCommand(
            Guid.NewGuid(),
            new PlaceOrderAddress(
                "Istanbul",
                "Sultanbeyli",
                "Test Street"),
            [
                new PlaceOrderItem(
                    Guid.NewGuid(),
                    0)
            ]);

        // Act
        var result = await validator.ValidateAsync(command);

        // Assert
        Assert.False(result.IsValid);

        Assert.Contains(
            result.Errors,
            error =>
                error.PropertyName.EndsWith(
                    nameof(PlaceOrderItem.Quantity)));
    }
}