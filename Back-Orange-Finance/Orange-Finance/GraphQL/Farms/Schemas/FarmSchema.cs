using GraphQL.Types;

using OrangeFinance.GraphQL.Farms.Queries;


namespace OrangeFinance.GraphQL.Farms.Schemas;

public class FarmSchema : Schema
{
    public FarmSchema(IServiceProvider resolver) : base(resolver)
    {
        Query = resolver.GetRequiredService<FarmQueryGraph>();
    }
}
