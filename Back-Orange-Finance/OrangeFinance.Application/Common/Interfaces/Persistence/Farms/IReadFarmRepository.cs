using OrangeFinance.Domain.Farms;

namespace OrangeFinance.Application.Common.Interfaces.Persistence.Farms;

public interface IReadFarmRepository
{
    Task<Farm> GetByIdAsync(Guid id);
    Task<IEnumerable<Farm>> GetAllAsync();
}
