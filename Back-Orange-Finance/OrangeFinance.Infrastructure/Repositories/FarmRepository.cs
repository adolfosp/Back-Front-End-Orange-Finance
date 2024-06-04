﻿using Microsoft.EntityFrameworkCore;

using OrangeFinance.Application.Common.Interfaces.Persistence.Farms;
using OrangeFinance.Domain.Common.Models;
using OrangeFinance.Domain.Farms.Models;
using OrangeFinance.Infrastructure.Extensions;
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

    public async Task<IEnumerable<FarmModel>> GetAllAsync(Pagination pagination)
    {
        return await _dbContext.Farms.Paginate(pagination).ToListAsync();
    }

    public async Task<FarmModel> GetByIdAsync(Guid id)
    {
        return await _dbContext.Farms.FirstOrDefaultAsync(x => x.Id == id);
    }

    public Task<FarmModel> UpdateAsync(FarmModel farm, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
