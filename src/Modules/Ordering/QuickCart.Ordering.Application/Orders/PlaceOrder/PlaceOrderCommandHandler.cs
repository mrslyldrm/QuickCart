using MediatR;
using QuickCart.Ordering.Application.Abstractions.Catalog;
using QuickCart.Ordering.Application.Abstractions.Persistence;
using QuickCart.Ordering.Domain.Orders;
using QuickCart.Ordering.Domain.ValueObjects;

namespace QuickCart.Ordering.Application.Orders.PlaceOrder;

public sealed class PlaceOrderCommandHandler
    : IRequestHandler<PlaceOrderCommand, Guid>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IProductCatalog _productCatalog;
    private readonly TimeProvider _timeProvider;

    public PlaceOrderCommandHandler(
        IOrderRepository orderRepository,
        IUnitOfWork unitOfWork,
        IProductCatalog productCatalog,
        TimeProvider timeProvider)
    {
        _orderRepository = orderRepository;
        _unitOfWork = unitOfWork;
        _productCatalog = productCatalog;
        _timeProvider = timeProvider;
    }

    public async Task<Guid> Handle(PlaceOrderCommand request, CancellationToken cancellationToken)
    {
        Guid[] productIds = request.Items
            .Select(x => x.ProductId)
            .Distinct()
            .ToArray();

        IReadOnlyCollection<ProductSnapshot> products =
            await _productCatalog.GetProductsAsync(
                productIds,
                cancellationToken);

        Dictionary<Guid, ProductSnapshot> productMap = products.ToDictionary(x => x.Id);

        Address shippingAddress = new(
            request.Address.City,
            request.Address.District,
            request.Address.Line,
            request.Address.PostalCode);

        Order order = Order.Create(
            Guid.CreateVersion7(),
            request.CustomerId,
            shippingAddress,
            _timeProvider.GetUtcNow());

        foreach (PlaceOrderItem requestItem in request.Items)
        {
            if (!productMap.TryGetValue(requestItem.ProductId, out ProductSnapshot? product))
            {
                throw new ProductUnavailableException(requestItem.ProductId);
            }

            if (!product.IsActive)
            {
                throw new ProductUnavailableException(
                    requestItem.ProductId);
            }

            Money price = new(
                product.Price,
                product.Currency);

            order.AddItem(
                product.Id,
                product.Name,
                price,
                requestItem.Quantity);
        }

        order.Place(_timeProvider.GetUtcNow());

        await _orderRepository.AddAsync(order, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return order.Id;
    }
}