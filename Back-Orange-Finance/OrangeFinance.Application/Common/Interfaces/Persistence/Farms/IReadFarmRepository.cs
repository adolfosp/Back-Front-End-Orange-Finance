using OrangeFinance.Domain.Farms.Models;

namespace OrangeFinance.Application.Common.Interfaces.Persistence.Farms;

public interface IReadFarmRepository
{
    Task<FarmModel> GetByIdAsync(Guid id);
    Task<IEnumerable<FarmModel>> GetAllAsync();
}
