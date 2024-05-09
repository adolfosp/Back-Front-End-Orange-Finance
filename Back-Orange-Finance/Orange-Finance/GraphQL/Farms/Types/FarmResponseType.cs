using GraphQL.Types;

using OrangeFinance.Contracts.Farms;

namespace OrangeFinance.GraphQL.Farms.Types;

public class FarmResponseType : ObjectGraphType<FarmResponse>
{
    public FarmResponseType()
    {
        Field(x => x.Name).Description("Nome da fazenda");
        Field(x => x.Description).Description("Descrição da fazenda");
        Field(x => x.Longitude).Description("Longitude da fazenda");
        Field(x => x.Latitude).Description("Latitude da fazenda");
        Field(x => x.Size).Description("Tamanho da fazenda");
        Field(x => x.Type).Description("Tipo da fazenda");
    }

}
