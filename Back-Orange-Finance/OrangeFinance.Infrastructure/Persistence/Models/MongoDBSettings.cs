namespace OrangeFinance.Infrastructure.Persistence.Models;

public class MongoDBSettings
{
    public static string SectionName { get => "MongoDBSettings"; }
    public string ConnectionString { get; set; }
    public string DatabaseName { get; set; }
}
