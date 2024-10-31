using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace OrangeFinance.Infrastructure.Persistence.Configurations;

public static class CreateDatabase
{
    public static void EnsureCreatedDatabase(
     this WebApplication web)
    {
        using (var scope = web.Services.CreateScope())
        {
            try
            {
                using var context = scope.ServiceProvider.GetRequiredService<OrangeFinanceDbContext>();
                context.Database.EnsureCreated();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
