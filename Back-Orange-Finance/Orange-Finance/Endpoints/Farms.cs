using MapsterMapper;

using MediatR;

using Microsoft.AspNetCore.Mvc;

using OrangeFinance.Application.Farms.Commands.CreateFarm;
using OrangeFinance.Application.Farms.Queries.Farms;
using OrangeFinance.Contracts.Farms;
using OrangeFinance.Domain.Common.Models;
using OrangeFinance.Extensions;

namespace OrangeFinance.Endpoints;

public static class Farms
{
    public static void RegisterFarmEndpoints(this IEndpointRouteBuilder routes)
    {

        var farms = routes.MapGroup("/farms");

        farms.MapPost("", async (IMediator mediator, IMapper mapper, [FromBody] CreateFarmRequest request) =>
        {
            //TODO: Validar envio de imagem da terra, criar fila, workers, adicionar cnpj (object value)
            //TODO: Adicionar validação de coordenadas. FluentValidation

            var command = mapper.Map<CreateFarmCommand>(request);

            var result = await mediator.Send(command);

            return result.Match(value => Results.Created("", value: mapper.Map<FarmResponse>(value)),
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

        farms.MapDelete("{id:int}", (int id) =>
        {

        }).Produces(statusCode: 400)
          .Produces(statusCode: 200)
          .MapToApiVersion(1)
          .WithOpenApi();
    }
}
