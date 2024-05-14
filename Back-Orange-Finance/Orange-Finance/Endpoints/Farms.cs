using MapsterMapper;

using MediatR;

using Microsoft.AspNetCore.Mvc;

using OrangeFinance.Application.Farms.Commands.CreateFarm;
using OrangeFinance.Application.Farms.Queries.Farms;
using OrangeFinance.Contracts.Farms;
using OrangeFinance.Extensions;

namespace OrangeFinance.Endpoints;

public static class Farms
{
    public static void RegisterUserEndpoints(this IEndpointRouteBuilder routes)
    {
        var farms = routes.MapGroup("/api/farms");

        farms.MapPost("", async (IMediator mediator, IMapper mapper, [FromBody] CreateFarmRequest request) =>
        {
            var command = mapper.Map<CreateFarmCommand>(request);

            var result = await mediator.Send(command);


            return result.Match(value => Results.Created("", value: mapper.Map<FarmResponse>(value)),
                           errors => errors.GetProblemsDetails());

        }).Produces(statusCode: 400)
          .Produces(statusCode: 201);


        farms.MapGet("", async (IMediator mediator, IMapper mapper) =>
        {
            var result = await mediator.Send(new FarmQuery());

            return result.Match(value => Results.Ok(mapper.Map<List<FarmResponse>>(value)),
                                errors => errors.GetProblemsDetails());
        }).Produces(statusCode: 400)
          .Produces(statusCode: 200);
    }
}
