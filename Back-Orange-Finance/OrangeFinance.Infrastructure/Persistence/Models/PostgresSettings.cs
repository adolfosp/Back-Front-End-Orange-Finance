namespace OrangeFinance.Infrastructure.Persistence.Models;

internal class PostgresSettings

{
    public static string SectionName { get => "PostgresSettings"; }
    public string ConnectionString { get; set; }
}
