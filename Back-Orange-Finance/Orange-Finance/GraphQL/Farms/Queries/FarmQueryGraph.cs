using GraphQL;
using GraphQL.Types;

using MapsterMapper;

using OrangeFinance.Application.Common.Interfaces.Persistence.Farms;
using OrangeFinance.Contracts.Farms;
using OrangeFinance.Domain.Common.Models;
using OrangeFinance.GraphQL.Farms.Types;

namespace OrangeFinance.GraphQL.Farms.Queries;

public sealed class FarmQueryGraph : ObjectGraphType
{
    public FarmQueryGraph(IReadFarmRepository readFarmRepository, IMapper mapper)
    {
        Field<ListGraphType<FarmResponseType>>("farms")
                .Description("Get all farms")
                .Argument<NonNullGraphType<IntGraphType>>("page", "index of current page")
                .Argument<NonNullGraphType<IntGraphType>>("pageSize", "how many register")
                .ResolveAsync(async context =>
                {
                    var page = context.GetArgument<int>("page");
                    var pageSize = context.GetArgument<int>("pageSize");
                    var pagination = new Pagination(page, pageSize);

                    return mapper.Map<IEnumerable<FarmResponse>>(await readFarmRepository.GetAllAsync(pagination));
                });

        Field<FarmResponseType>(
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