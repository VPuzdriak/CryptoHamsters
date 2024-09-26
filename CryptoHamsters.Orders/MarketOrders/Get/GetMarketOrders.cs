using CryptoHamsters.Orders.Domain;
using CryptoHamsters.Orders.Infrastructure;

using MediatR;

namespace CryptoHamsters.Orders.MarketOrders.Get;

public record GetMarketOrders(Guid WalletId) : IRequest<IReadOnlyList<MarketOrder>>;

internal sealed class GetMarketOrdersHandler(IMarketOrderRepository marketOrderRepository)
    : IRequestHandler<GetMarketOrders, IReadOnlyList<MarketOrder>>
{
    public Task<IReadOnlyList<MarketOrder>> Handle(GetMarketOrders request, CancellationToken cancellationToken) =>
        marketOrderRepository.GetAsync(request.WalletId, cancellationToken);
}