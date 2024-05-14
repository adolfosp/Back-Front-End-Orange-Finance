using GraphQL;
using GraphQL.Types;

using MapsterMapper;

using MediatR;

using OrangeFinance.Application.Farms.Commands.CreateFarm;
using OrangeFinance.Contracts.Farms;
using OrangeFinance.GraphQL.Farms.Types;

namespace OrangeFinance.GraphQL.Farms.Mutations;

public sealed class FarmMutationGraph : ObjectGraphType<object>
{
    public FarmMutationGraph(IMapper mapper, IMediator mediator)
    {
        Field<FarmResponseType>(
             "createFarm")
            .Description("Create farm")
            .Argument<FarmInputType>("farm", "The farm to create")
            .ResolveAsync(async context =>
            {
                try
                {
                    var farmRequest = context.GetArgument<CreateFarmRequest>("farm");
                    var command = mapper.Map<CreateFarmCommand>(farmRequest);
                    var response = await mediator.Send(command);

                    if (response.IsError)
                    {
                        context.Errors.Add(new ExecutionError(response.FirstError.Description));
                        return null;
                    }

                    return mapper.Map<FarmResponse>(response.Value);
                }
                catch (Exception ex)
                {
                    context.Errors.Add(new ExecutionError(ex.Message));
                    return null;
                }
            }
         );
    }
}
