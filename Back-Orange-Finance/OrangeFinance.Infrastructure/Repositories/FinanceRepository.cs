using OrangeFinance.Application.Common.Interfaces.Persistence.Finances;
using OrangeFinance.Domain.Finances.Models;
using OrangeFinance.Infrastructure.Persistence;

namespace OrangeFinance.Infrastructure.Repositories;

internal sealed class FinanceRepository(OrangeFinanceDbContext context) : IWriteFinanceRepository
{
    private readonly OrangeFinanceDbContext _dbContext = context;

    public async Task AddAsync(FinanceModel finance, CancellationToken cancellationToken)
    {
        await _dbContext.Finances.AddAsync(finance, cancellationToken);
    }
}
