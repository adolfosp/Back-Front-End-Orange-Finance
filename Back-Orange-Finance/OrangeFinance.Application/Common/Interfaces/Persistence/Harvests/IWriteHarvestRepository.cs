using OrangeFinance.Domain.Harvests.Models;

namespace OrangeFinance.Application.Common.Interfaces.Persistence.Harvests;

public interface IWriteHarvestRepository
{
    Task AddAsync(HarvestModel harvest, CancellationToken cancellationToken);
}
