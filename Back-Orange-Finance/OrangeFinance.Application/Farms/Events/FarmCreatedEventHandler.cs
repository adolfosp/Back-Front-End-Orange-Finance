using MediatR;

using OrangeFinance.Application.Common.Interfaces.Persistence.DomainEvents;
using OrangeFinance.Domain.Farms.Events;

namespace OrangeFinance.Application.Farms.Events;

public sealed class FarmCreatedEventHandler : INotificationHandler<FarmCreated>
{
    private readonly IWriteDomainEventsRepository _writeDomainEventsRepository;

    public FarmCreatedEventHandler(IWriteDomainEventsRepository writeDomainEventsRepository)
    {
        _writeDomainEventsRepository = writeDomainEventsRepository;
    }

    public Task Handle(FarmCreated notification, CancellationToken cancellationToken)
    {
#warning Add queue to send errors with the event
        _ = _writeDomainEventsRepository.AddAsync(notification);
        return Task.CompletedTask;
    }
}