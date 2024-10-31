using Polly;

namespace OrangeFinance.Extensions;

internal static class HttpClients
{
    public static void AddClientsFactory(this WebApplicationBuilder builder)
    {
        const int retryCount = 3;
        HttpMethod[] idempotentMethod = [HttpMethod.Get, HttpMethod.Put, HttpMethod.Delete];

        builder.Services.AddHttpClient("Back-Authentication", client =>
        {
            client.BaseAddress = new Uri("http://127.0.0.1:3000");
            client.DefaultRequestHeaders.Add("Origin", "Orange-Finance-Api");

        }).AddPolicyHandler(request =>
        {

            if (idempotentMethod.Contains(request.Method))
            {
                return Policy<HttpResponseMessage>
                    .Handle<HttpRequestException>()
                    .OrResult(r => !r.IsSuccessStatusCode)
                    .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                                       onRetry: (outcome, timespan, retryCount, context) =>
                                       {
                                           Console.WriteLine($"Retry {retryCount} falhou. Tentando novamente em {timespan}.");
                                       });
            }

            return Policy.NoOpAsync<HttpResponseMessage>();
        }).
        AddPolicyHandler(Policy<HttpResponseMessage>
            .Handle<HttpRequestException>()
            .OrResult(r => !r.IsSuccessStatusCode)
            .CircuitBreakerAsync(retryCount, TimeSpan.FromMinutes(1),
                                 onBreak: (outcome, breakDelay) =>
                                 {
                                     Console.WriteLine($"Circuito aberto devido a {retryCount} falhas consecutivas.");
                                 },
                                 onReset: () =>
                                 {
                                     Console.WriteLine("Circuito fechado. Retomando tentativas.");
                                 }));
    }
}
