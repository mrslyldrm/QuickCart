using FluentValidation;
using MediatR;

namespace QuickCart.Ordering.Application.Behaviors;

public sealed class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        IValidator<TRequest>[] validators = _validators.ToArray();

        if (validators.Length == 0)
        {
            return await next();
        }

        ValidationContext<TRequest> context = new(request);

        FluentValidation.Results.ValidationResult[] results =
            await Task.WhenAll(
                validators.Select(
                    validator =>
                        validator.ValidateAsync(
                            context,
                            cancellationToken)));

        FluentValidation.Results.ValidationFailure[] failures =
            results
                .SelectMany(x => x.Errors)
                .Where(x => x is not null)
                .ToArray();

        if (failures.Length != 0)
        {
            throw new ValidationException(failures);
        }

        return await next();
    }
}