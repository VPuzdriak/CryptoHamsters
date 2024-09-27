using CryptoHamsters.Shared.Marten;

namespace CryptoHamsters.Wallets.Domain;

public sealed class Wallet : IAggregate
{
    public Guid Id { get; init; }
    public Guid CustomerId { get; init; }
    public WalletType Type { get; init; }
    public DateTime CreatedAtUtc { get; init; }
    public HashSet<WalletAsset> Assets { get; init; }
    public long Version { get; set; }

    private Wallet()
    {
        Id = Guid.Empty;
        CustomerId = Guid.Empty;
        Type = WalletType.Unknown;
        CreatedAtUtc = DateTime.MinValue;
        Assets = [];
        Version = -1;
    }

    private Wallet(Guid id, Guid customerId, WalletType type, DateTime createdAtUtc)
    {
        Id = id;
        CustomerId = customerId;
        Type = type;
        CreatedAtUtc = createdAtUtc;
        Assets = [];
        Version = 1;
    }

    public static Wallet Create(WalletCreated @event) =>
        new(@event.Id, @event.CustomerId, @event.Type, @event.CreatedAtUtc);

    public bool CanWithdrawAsset(WalletAsset asset) =>
        Assets.Any(a => a.Name == asset.Name && a.Amount >= asset.Amount);

    private void Apply(WalletToppedUp @event)
    {
        var asset = Assets.FirstOrDefault(a => a.Name == @event.Asset.Name);

        asset = asset is null
            ? new WalletAsset(@event.Asset.Name, @event.Asset.Amount)
            : asset with { Amount = asset.Amount + @event.Asset.Amount };

        Assets.RemoveWhere(a => a.Name == @event.Asset.Name);
        Assets.Add(asset);

        Version++;
    }

    private void Apply(WalletAssetWithdrawn @event)
    {
        var asset = Assets.FirstOrDefault(a => a.Name == @event.Asset.Name);

        if (asset is null)
        {
            return;
        }

        asset = asset with { Amount = asset.Amount - @event.Asset.Amount };

        Assets.RemoveWhere(a => a.Name == @event.Asset.Name);
        Assets.Add(asset);

        Version++;
    }
}