using OrangeFinance.Application.Common.Interfaces.Persistence.Harvests;
using OrangeFinance.Domain.Harvests.Models;
using OrangeFinance.Infrastructure.Persistence;

namespace OrangeFinance.Infrastructure.Repositories;

internal sealed class HarvestRepository(OrangeFinanceDbContext dbContext) : IWriteHarvestRepository
{
    private readonly OrangeFinanceDbContext _dbContext = dbContext;

    public async Task AddAsync(HarvestModel harvest, CancellationToken cancellationToken)
    {
        await _dbContext.AddAsync(harvest, cancellationToken);
    }
}
