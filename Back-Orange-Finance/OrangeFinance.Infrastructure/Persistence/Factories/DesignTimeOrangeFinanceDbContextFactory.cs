using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using OrangeFinance.Infrastructure.Persistence.Settings;

namespace OrangeFinance.Infrastructure.Persistence.Factories;

/// <summary>
/// Classe criada pelo motivo de que o EFCore não consgue resolver as dependências extras do DbContext em tempo de design.
/// Quando rodamos a migration, o EFCore lê esse contexto
/// </summary>
public class DesignTimeOrangeFinanceDbContextFactory : IDesignTimeDbContextFactory<OrangeFinanceDbContext>
{
    public OrangeFinanceDbContext CreateDbContext(string[] args)
    {
        var relativePath = Path.Combine("..", "Orange-Finance");
        var basePath = Path.GetFullPath(relativePath);

        var configuration = new ConfigurationBuilder()
            .SetBasePath(basePath)
            .AddJsonFile("appsettings.json", optional: false)
            .Build();

        var optionsBuilder = new DbContextOptionsBuilder<OrangeFinanceDbContext>();
        var postgresSettings = new PostgresSettings();
        configuration.Bind(PostgresSettings.SectionName, postgresSettings);

        Console.WriteLine($"Using PostgreSQL connection string: {postgresSettings.ConnectionString}");

        optionsBuilder.UseNpgsql(postgresSettings.ConnectionString.Replace("Host=postgres", "Host=localhost"));

        return new OrangeFinanceDbContext(optionsBuilder.Options, null);
    }
}

