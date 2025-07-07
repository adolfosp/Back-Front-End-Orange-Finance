namespace OrangeFinance.Infrastructure.Persistence.Settings;

internal class RedisSettings
{
    public static string SectionName { get => "RedisSettings"; }
    public string ConnectionString { get; set; }

}
