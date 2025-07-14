using OrangeFinance.Application.Common.Interfaces.Persistence.Harvests;
using OrangeFinance.Domain.Harvests.Models;

namespace OrangeFinance.Infrastructure.Repositories;

internal sealed class HarvestRepository : IWriteHarvestRepository
{
    public Task AddAsync(HarvestModel harvest, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
