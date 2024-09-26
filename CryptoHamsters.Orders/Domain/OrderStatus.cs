namespace CryptoHamsters.Orders.Domain;

public enum OrderStatus
{
    Unknown = 0,
    Placed = 1,
    Fulfilled = 2,
    Cancelled = 3
}