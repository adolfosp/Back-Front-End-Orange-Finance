using MapsterMapper;

using MediatR;

using Microsoft.AspNetCore.Mvc;

using OrangeFinance.Application.Amqp;
using OrangeFinance.Application.Farms.Commands.CreateFarm;
using OrangeFinance.Application.Farms.Commands.DeleteFarm;
using OrangeFinance.Application.Farms.Queries.Farms;
using OrangeFinance.Contracts.Farms;
using OrangeFinance.Domain.Common.Models;
using OrangeFinance.Extensions;

namespace OrangeFinance.Endpoints;

/// <summary>
/// Abordagem usada para registrar os endpoints relacionados às fazendas.
/// MediaTR é usado para manipular comandos e consultas, enquanto MapsterMapper é usado para mapear entre DTOs e modelos de domínio.
/// Minimal APIs são usadas para definir os endpoints.
/// </summary>
public static class Farms
{
    public static void RegisterFarmEndpoints(this IEndpointRouteBuilder routes)
    {

        var farms = routes.MapGroup("/farms");

        farms.MapPost("", async (IMediator mediator, IMapper mapper, ILogger<string> logger, AmqpFarmService aqmpFarm, [FromBody] CreateFarmRequest request) =>
        {

            //WIP: Validar envio de imagem da terra, criar fila, workers, adicionar cnpj (object value)
            //TODO: Adicionar validação de coordenadas. FluentValidation

            var command = mapper.Map<CreateFarmCommand>(request);

            var result = await mediator.Send(command);

            return result.Match(value =>
            {
                aqmpFarm.SendLocationFarm(new { value.Location.Latitude, value.Location.Longitude });
                logger.LogInformation("Farm created with ID: {FarmId}", value.Id.Value);
                return Results.Created("", value: mapper.Map<FarmResponse>(value));
            },
            errors => errors.GetProblemsDetails());

        }).Produces(statusCode: 400)
          .Produces(statusCode: 201)
          .MapToApiVersion(1)
          .WithOpenApi();

        farms.MapGet("", async (IMediator mediator, IMapper mapper, [AsParameters] Pagination pagination) =>
        {
            var result = await mediator.Send(new FarmQuery(pagination));

            return result.Match(value => Results.Ok(mapper.Map<List<FarmResponse>>(value)),
                                errors => errors.GetProblemsDetails());
        }).Produces(statusCode: 400)
          .Produces(statusCode: 200)
          .MapToApiVersion(1)
          .WithOpenApi();

        farms.MapDelete("{id:Guid}", async (Guid id, IMediator mediator) =>
        {
            var command = new DeleteFarmCommand(id);

            var result = await mediator.Send(command);

            return result.Match(value =>
            {
                if (value)
                    return Results.Ok();

                return Results.NotFound();
            },
            errors => errors.GetProblemsDetails());


        }).Produces(statusCode: 400)
          .Produces(statusCode: 200)
          .MapToApiVersion(1)
          .WithOpenApi();
    }
}
