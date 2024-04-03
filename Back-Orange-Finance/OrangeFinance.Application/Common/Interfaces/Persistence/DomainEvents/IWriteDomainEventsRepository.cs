using OrangeFinance.Domain.Farms.Events;

namespace OrangeFinance.Application.Common.Interfaces.Persistence.DomainEvents;

public interface IWriteDomainEventsRepository
{
    Task AddAsync(FarmCreated notification);
}
