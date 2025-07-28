using OrangeFinance.Domain.Finances.Models;

namespace OrangeFinance.Application.Common.Interfaces.Persistence.Finances;

public interface IWriteFinanceRepository
{
    Task AddAsync(FinanceModel finance, CancellationToken cancellationToken);

}
