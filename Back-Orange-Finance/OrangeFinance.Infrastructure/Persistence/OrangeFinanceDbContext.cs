

using Microsoft.EntityFrameworkCore;
using OrangeFinance.Application.Common.Interfaces.Persistence;
using OrangeFinance.Domain.Common.Models;
using OrangeFinance.Domain.Farms.Models;
using OrangeFinance.Domain.Finances.Models;
using OrangeFinance.Domain.Harvests.Models;
using OrangeFinance.Infrastructure.Persistence.Interceptors;

namespace OrangeFinance.Infrastructure.Persistence;

/// <summary>
/// A interface IUnitOfWork funciona porque DbContext já implementa SaveChangesAsync(), que está em IUnitOfWork. Métodos abaixos são permitidos:
/// <code>
/// Task BeginTransactionAsync(CancellationToken cancellationToken = default);
/// Task CommitAsync(CancellationToken cancellationToken = default);
/// Task RollbackAsync(CancellationToken cancellationToken = default);
/// </code>
/// </summary>
public sealed class OrangeFinanceDbContext : DbContext, IUnitOfWork
{
    private readonly PublishDomainEventsInterceptor? _publishDomainEventsInterceptor;

    public OrangeFinanceDbContext(
        DbContextOptions<OrangeFinanceDbContext> options,
        PublishDomainEventsInterceptor? publishDomainEventsInterceptor
    ) : base(options)
    {
        _publishDomainEventsInterceptor = publishDomainEventsInterceptor;
    }

    public DbSet<FarmModel> Farms { get; set; }

    public DbSet<HarvestModel> Harvests { get; set; }

    public DbSet<FinanceModel> Finances { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .Ignore<List<IDomainEvent>>()
            .ApplyConfigurationsFromAssembly(typeof(OrangeFinanceDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder
            .AddInterceptors(_publishDomainEventsInterceptor);

        base.OnConfiguring(optionsBuilder);
    }
}