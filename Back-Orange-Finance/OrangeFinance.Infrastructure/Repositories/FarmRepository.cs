using OrangeFinance.Application.Common.Interfaces.Persistence.Farms;
using OrangeFinance.Domain.Farms.Models;
using OrangeFinance.Infrastructure.Persistence;

namespace OrangeFinance.Infrastructure.Repositories;

internal sealed class FarmRepository : IReadFarmRepository, IWriteFarmRepository
{
    private readonly OrangeFinanceDbContext _dbContext;

    public FarmRepository(OrangeFinanceDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(FarmModel farm, CancellationToken cancellationToken)
    {
        _dbContext.Add(farm);
        await _dbContext.SaveChangesAsync();
    }

    public Task DeleteAsync(Guid farmId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<FarmModel>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<FarmModel> GetByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<FarmModel> UpdateAsync(FarmModel farm, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
