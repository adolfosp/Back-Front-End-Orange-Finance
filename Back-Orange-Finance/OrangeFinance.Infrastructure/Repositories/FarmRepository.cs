using OrangeFinance.Application.Common.Interfaces.Persistence.Farms;
using OrangeFinance.Domain.Farms;

namespace OrangeFinance.Infrastructure.Repositories;

internal sealed class FarmRepository : IReadFarmRepository, IWriteFarmRepository
{
    public Task<Farm> AddAsync(Farm farm, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(Farm farm, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Farm>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Farm> GetByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<Farm> UpdateAsync(Farm farm, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
