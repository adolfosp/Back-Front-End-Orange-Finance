using MongoDB.Bson;

using OrangeFinance.Application.Common.Interfaces.Persistence.DomainEvents;
using OrangeFinance.Domain.Farms.Events;
using OrangeFinance.Infrastructure.Persistence;

namespace OrangeFinance.Infrastructure.Repositories;

internal sealed class DomainEventsRepository : IWriteDomainEventsRepository
{
    private readonly MongoDBContext _context;
    private readonly string _collection = "DomainEvents";

    public DomainEventsRepository(MongoDBContext context)
    {
        _context = context;
    }

    public async Task AddAsync(FarmCreated @event)
    {
        await _context.MongoDB.GetCollection<BsonDocument>(_collection).InsertOneAsync(@event.ToBsonDocument());
    }
}
