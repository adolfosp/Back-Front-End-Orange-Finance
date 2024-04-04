namespace OrangeFinance.Infrastructure.Persistence.Models;

internal class MongoDBSettings
{
    public static string SectionName { get => "MongoDBSettings"; }
    public string ConnectionString { get; set; }
    public string DatabaseName { get; set; }
}
