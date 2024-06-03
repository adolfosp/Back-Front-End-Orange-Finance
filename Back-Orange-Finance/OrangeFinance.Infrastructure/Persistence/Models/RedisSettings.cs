namespace OrangeFinance.Infrastructure.Persistence.Models;

internal class RedisSettings
{
    public static string SectionName { get => "RedisSettings"; }
    public string ConnectionString { get; set; }

}
