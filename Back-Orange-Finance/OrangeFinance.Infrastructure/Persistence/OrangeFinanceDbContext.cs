

using Microsoft.EntityFrameworkCore;

using OrangeFinance.Domain.Common.Models;
using OrangeFinance.Domain.Farms.Models;
using OrangeFinance.Infrastructure.Persistence.Interceptors;

namespace OrangeFinance.Infrastructure.Persistence;

public sealed class OrangeFinanceDbContext : DbContext
{
    private readonly PublishDomainEventsInterceptor _publishDomainEventsInterceptor;

    public OrangeFinanceDbContext(
        DbContextOptions<OrangeFinanceDbContext> options,
        PublishDomainEventsInterceptor publishDomainEventsInterceptor
    ) : base(options)
    {
        _publishDomainEventsInterceptor = publishDomainEventsInterceptor;
    }

    public DbSet<FarmModel> Bills { get; set; } = null!;

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