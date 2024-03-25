namespace OrangeFinance.Endpoints;

public static class Farms
{
    public static void RegisterUserEndpoints(this IEndpointRouteBuilder routes)
    {
        var farms = routes.MapGroup("/api/farms");

        farms.MapPost("", null);
        farms.MapGet("", null);
        farms.MapGet("/{id}", null);


    }
}
