namespace OrangeFinance.Infrastructure.Persistence.Settings;

internal class PostgresSettings

{
    public static string SectionName { get => "PostgresSettings"; }
    public string ConnectionString { get; set; }
}
