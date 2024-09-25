namespace CryptoHamsters.CryptoPairs.Domain;

public sealed class CryptoPair
{
    public Guid Id { get; init; }
    public string Ticker { get; init; }
    public string BaseAsset { get; init; }
    public string QuoteAsset { get; init; }
    public DateTime ListedAtUtc { get; init; }
    public decimal Price { get; set; }
    public DateTime PriceUpdatedAtUtc { get; set; }
    public long Version { get; set; }

    private CryptoPair()
    {
        Id = Guid.Empty;
        Ticker = string.Empty;
        BaseAsset = string.Empty;
        QuoteAsset = string.Empty;
        ListedAtUtc = DateTime.MinValue;
        Price = 0;
        PriceUpdatedAtUtc = DateTime.MinValue;
        Version = -1;
    }

    private CryptoPair(Guid id, string ticker, string baseAsset, string quoteAsset, DateTime listedAtUtc, decimal price)
    {
        Id = id;
        Ticker = ticker;
        BaseAsset = baseAsset;
        QuoteAsset = quoteAsset;
        ListedAtUtc = listedAtUtc;
        Price = price;
        PriceUpdatedAtUtc = listedAtUtc;
        Version = 1;
    }

    public static CryptoPair Create(CryptoPairListed @event) =>
        new(@event.Id, @event.Ticker, @event.BaseAsset, @event.QuoteAsset, @event.ListedAtUtc, @event.Price);
}