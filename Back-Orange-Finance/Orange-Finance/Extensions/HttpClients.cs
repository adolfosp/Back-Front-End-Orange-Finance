namespace OrangeFinance.Extensions;

internal static class HttpClients
{
    public static void AddClientsFactory(this WebApplicationBuilder builder)
    {
        builder.Services.AddHttpClient("Back-Authentication", client =>
        {
            client.BaseAddress = new Uri("https://localhost:5001");
        });
    }
}
