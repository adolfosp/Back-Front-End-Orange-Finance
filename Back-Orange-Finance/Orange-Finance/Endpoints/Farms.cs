using ErrorOr;

using MapsterMapper;

using MediatR;

using Microsoft.AspNetCore.Mvc;

using OrangeFinance.Application.Farms.Commands.CreateFarm;
using OrangeFinance.Contracts.Farms;
using OrangeFinance.Extensions;

namespace OrangeFinance.Endpoints;

public static class Farms
{
    public static void RegisterUserEndpoints(this IEndpointRouteBuilder routes)
    {
        var farms = routes.MapGroup("/api/farms");

        farms.MapPost("", (IMediator mediator, IMapper mapper, [FromBody] CreateFarmRequest request) =>
        {
            var command = mapper.Map<CreateFarmCommand>(request);

            var result = mediator.Send(command);


            return result.Match(value => Results.Created("", value: mapper.Map<FarmResponse>(value)),
                           errors => errors.GetProblemsDetails());

        }).Produces(statusCode: 400)
          .Produces(statusCode: 200);


        //farms.MapGet("", null);
        //farms.MapGet("/{id}", null);

        //TODO: read repository, write repository, IDataEntity
    }
}
