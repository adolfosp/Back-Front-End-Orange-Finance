namespace OrangeFinance.Infrastructure.Persistence.Models;

internal class SqlServerSettings
{
    public static string SectionName { get => "SqlServerSettings"; }
    public string ConnectionString { get; set; }
}
