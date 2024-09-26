namespace CryptoHamsters.Service.GraphQL.Orders.Market;

public record PlaceMarketOrderInput(Guid WalletId, Guid CryptoPairId, decimal QuoteAssetAmount);