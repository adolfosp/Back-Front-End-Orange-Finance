using OrangeFinance.Domain.Common.Models;
using OrangeFinance.Domain.Farms.Events;
using OrangeFinance.Domain.Farms.ValueObjects;

namespace OrangeFinance.Domain.Farms;

public sealed class Farm : AggregateRoot<FarmId, Guid>
{
    private Farm(string name, string description, Coordinate location, string size, string type, string image)
    {
        Name = name;
        Description = description;
        Location = location;
        Size = size;
        Type = type;
        Image = image;
    }

    public string Name { get; private set; }
    public string Description { get; private set; }
    public Coordinate Location { get; private set; }
    public string Size { get; private set; }
    public string Type { get; private set; }
    public string Image { get; private set; }

    public static Farm Create(string name, string description, Coordinate location, string size, string type, string image)
    {
        var farm = new Farm(name, description, location, size, type, image);
        farm.AddDomainEvent(new FarmCreated(farm));
        return farm;
    }

}
