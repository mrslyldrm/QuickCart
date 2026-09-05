using FluentValidation;

namespace QuickCart.Ordering.Application.Orders.PlaceOrder;

public sealed class PlaceOrderCommandValidator
    : AbstractValidator<PlaceOrderCommand>
{
    public PlaceOrderCommandValidator()
    {
        RuleFor(x => x.CustomerId)
            .NotEmpty();

        RuleFor(x => x.Address)
            .NotNull();

        RuleFor(x => x.Address.City)
            .NotEmpty()
            .When(x => x.Address is not null);

        RuleFor(x => x.Address.District)
            .NotEmpty()
            .When(x => x.Address is not null);

        RuleFor(x => x.Address.Line)
            .NotEmpty()
            .When(x => x.Address is not null);

        RuleFor(x => x.Items)
            .NotEmpty();

        RuleForEach(x => x.Items)
            .ChildRules(item =>
            {
                item.RuleFor(x => x.ProductId)
                    .NotEmpty();

                item.RuleFor(x => x.Quantity)
                    .GreaterThan(0);
            });
    }
}