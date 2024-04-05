using GraphQL;
using GraphQL.Types;

using MapsterMapper;

using OrangeFinance.Application.Common.Interfaces.Persistence.Farms;
using OrangeFinance.Contracts.Farms;
using OrangeFinance.GraphQL.Farms.Types;

namespace OrangeFinance.GraphQL.Farms.Queries;

public class FarmQueryGraph : ObjectGraphType
{
    public FarmQueryGraph(IReadFarmRepository readFarmRepository, IMapper mapper)
    {
        Field<ListGraphType<FarmType>>("farms")
                .Description("Get all farms")
                .ResolveAsync(async context => mapper.Map<IEnumerable<FarmResponse>>(await readFarmRepository.GetAllAsync()));

        Field<FarmType>(
           "farmById")
            .Description("Get a single farm by ID")
            .Argument<NonNullGraphType<IdGraphType>>("id", "The ID of the farm")
            .ResolveAsync(async context =>
            {
                var id = context.GetArgument<Guid>("id");
                var farm = await readFarmRepository.GetByIdAsync(id);
                if (farm == null)
                {
                    context.Errors.Add(new ExecutionError("Farm not found"));
                    return null;
                }
                return mapper.Map<FarmResponse>(farm);
            }
        );
    }
}