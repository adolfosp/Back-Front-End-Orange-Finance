using OrangeFinance.Domain.Farms;

namespace OrangeFinance.Application.Common.Interfaces.Persistence.Farms;

public interface IWriteFarmRepository
{
    Task<Farm> AddAsync(Farm farm, CancellationToken cancellationToken);
    Task<Farm> UpdateAsync(Farm farm, CancellationToken cancellationToken);
    Task DeleteAsync(Farm farm, CancellationToken cancellationToken);
}
