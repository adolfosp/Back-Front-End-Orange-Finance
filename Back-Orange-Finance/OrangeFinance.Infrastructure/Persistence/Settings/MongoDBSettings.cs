namespace OrangeFinance.Infrastructure.Persistence.Settings;

internal class MongoDBSettings
{
    public static string SectionName { get => "MongoDBSettings"; }
    public string ConnectionString { get; set; }
    public string DatabaseName { get; set; }
}
