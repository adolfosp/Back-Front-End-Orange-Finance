using MongoDB.Bson.Serialization;

using OrangeFinance.Domain.Farms.Events;

namespace OrangeFinance.Common.Mapping.MongoDB;

public static class MongoDBMappingConfig
{
    public static void RegisterMappings()
    {

        if (!BsonClassMap.IsClassMapRegistered(typeof(FarmCreated)))
        {
            BsonClassMap.RegisterClassMap<FarmCreated>(cm =>
            {
                cm.MapMember(c => c.Name).SetIsRequired(true);
                cm.MapMember(c => c.Description);
                cm.MapMember(c => c.Longitude);
                cm.MapMember(c => c.Latitude);
                cm.MapMember(c => c.Size);
                cm.MapMember(c => c.Type);
                cm.MapMember(c => c.Image);
            });
        }

    }
}
