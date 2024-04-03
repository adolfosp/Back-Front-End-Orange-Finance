using MongoDB.Driver;

namespace OrangeFinance.Infrastructure.Persistence;

internal sealed class MongoDBContext
{

    public readonly IMongoDatabase MongoDB;

    public MongoDBContext(string connectionString, string databaseName)
    {
        var client = new MongoClient(connectionString);
        MongoDB = client.GetDatabase(databaseName);
    }
}
