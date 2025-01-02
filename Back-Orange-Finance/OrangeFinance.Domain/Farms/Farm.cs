using OrangeFinance.Domain.Common.Models;
using OrangeFinance.Domain.Common.ValueObject;
using OrangeFinance.Domain.Farms.Events;
using OrangeFinance.Domain.Farms.ValueObjects;

namespace OrangeFinance.Domain.Farms;

public sealed class Farm : AggregateRoot<FarmId, Guid>
{
    private Farm(FarmId id, string name, string description, double longitude, double latitude, string size, string type, Cnpj cnpj)
    {
        Id = id;
        Name = name;
        Description = description;
        Location = new Coordinate(latitude: latitude, longitude: longitude);
        Size = size;
        Type = type;
        Cnpj = cnpj;
    }

    public string Name { get; private set; }
    public string Description { get; private set; }
    public Coordinate Location { get; private set; }
    public string Size { get; private set; }
    public string Type { get; private set; }
    public string Image { get; private set; }
    public Cnpj Cnpj { get; private set; }

    public static Farm Create(string name, string description, double longitude, double latitude, string size, string type, Cnpj cnpj)
    {
        var farm = new Farm(id: FarmId.CreateUnique(), name: name, description: description, longitude: longitude, latitude: latitude, size: size, type: type, cnpj: cnpj);
        farm.AddDomainEvent(new FarmCreated(farm.Id.Value, farm.Name, farm.Description, farm.Location.Longitude, farm.Location.Latitude, farm.Size, farm.Type, farm.Image));
        return farm;
    }

}
