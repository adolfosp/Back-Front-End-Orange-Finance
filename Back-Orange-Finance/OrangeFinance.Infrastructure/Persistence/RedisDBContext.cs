using StackExchange.Redis;

namespace OrangeFinance.Infrastructure.Persistence;

internal class RedisDBContext : IDisposable
{
    private ConnectionMultiplexer _redisConnection;
    public IDatabase Database { get; private set; }

    public RedisDBContext(string connectionString)
    {
        _redisConnection = ConnectionMultiplexer.Connect(connectionString);
        Database = _redisConnection.GetDatabase();
    }

    public void Dispose()
    {
        _redisConnection?.Dispose();
    }
}
