using OrangeFinance.Domain.Farms.Models;

namespace OrangeFinance.Application.Common.Interfaces.Persistence.Farms;

public interface IWriteFarmRepository
{
    Task AddAsync(FarmModel farm, CancellationToken cancellationToken);
    Task DeleteAsync(Guid farmId, CancellationToken cancellationToken);
}
