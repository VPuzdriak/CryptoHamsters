namespace CryptoHamsters.Shared.Marten;

public interface IAggregate
{
    public long Version { get; set; }
}