using MongoDB.Bson.Serialization;

using OrangeFinance.Domain.Farms.Events;

namespace OrangeFinance.Common.Mapping.MongoDB;

public class MappingConfig
{

    public static void Configurar()
    {

        BsonClassMap.RegisterClassMap<FarmCreated>(cm =>
        {
            cm.AutoMap();

        });

    }
}
