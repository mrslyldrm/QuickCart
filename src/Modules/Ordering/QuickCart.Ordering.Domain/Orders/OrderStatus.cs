namespace QuickCart.Ordering.Domain.Orders;

public enum OrderStatus
{
    Draft = 1,

    PendingStockReservation = 2,

    Confirmed = 3,

    Rejected = 4,

    Completed = 5,

    Cancelled = 6
}