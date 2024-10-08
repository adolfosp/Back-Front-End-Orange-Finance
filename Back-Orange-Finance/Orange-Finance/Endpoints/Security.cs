using Microsoft.AspNetCore.Mvc;

using OrangeFinance.Application.Security;
using OrangeFinance.Application.Security.Dtos;
using OrangeFinance.Extensions;

namespace OrangeFinance.Endpoints;

public static class Security
{
    public static void RegisterSecurityEndpoints(this IEndpointRouteBuilder routes)
    {
        var farms = routes.MapGroup("/security");

        farms.MapPost("token", async ([FromBody] LoginDto dto, SecurityService service) =>
        {
            var result = await service.GetApiTokenAsync(dto);

            return result.Match(value => Results.Ok(value: value),
                                errors => errors.GetProblemsDetails());

        }).Produces(statusCode: 400)
          .Produces(statusCode: 201)
          .MapToApiVersion(1)
          .WithOpenApi();
    }
}
