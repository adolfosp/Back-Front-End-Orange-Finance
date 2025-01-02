using GraphQL.Types;

using OrangeFinance.Contracts.Farms;

namespace OrangeFinance.GraphQL.Farms.Types;

public class FarmInputType : InputObjectGraphType<CreateFarmRequest>
{
    public FarmInputType()
    {
        Field<NonNullGraphType<StringGraphType>>("Name").Description("Nome da fazenda");
        Field(x => x.Description).Description("Descrição da fazenda");
        Field(x => x.Longitude).Description("Longitude da fazenda");
        Field(x => x.Latitude).Description("Latitude da fazenda");
        Field(x => x.Size).Description("Tamanho da fazenda");
        Field(x => x.Type).Description("Tipo da fazenda");

    }
}
