using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using OrangeFinance.Application.Harvests;
using OrangeFinance.Contracts.Harvests;
using OrangeFinance.Domain.Harvests;
using OrangeFinance.Extensions;
using System.ComponentModel.DataAnnotations;

namespace OrangeFinance.Endpoints;

/// <summary>
/// 1- Abordagem usada para registrar os endpoints relacionados às fazendas.
/// 2- Usa o conceito de serviços (AppService) para trabalhar com os dados
/// 3- Minimal APIs são usadas para definir os endpoints.
/// 4- Validações da DTO são feitas usando o Data Annotations.
/// </summary>
public static class Harvests
{
    public static void RegisterHarvestEndpoints(this IEndpointRouteBuilder routes)
    {
        var farms = routes.MapGroup("/harvests");

        farms.MapPost("", async (IMediator mediator, IMapper mapper, ILogger<string> logger, HarvestsAppService service, [FromBody] HarvestsDto request) =>
        {
            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(request, new ValidationContext(request), validationResults, true);

            if (!isValid)
                return validationResults.GetProblemsDetails();

            var harvestModel = mapper.Map<Harvest>(request);

            await service.CreateHarvestAsync(harvestModel);

            return Results.Created();

        }).Produces(statusCode: 400)
          .Produces(statusCode: 201)
          .MapToApiVersion(1)
          .WithOpenApi();
    }
}
