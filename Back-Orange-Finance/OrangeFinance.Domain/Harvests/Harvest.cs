using OrangeFinance.Domain.Common.Models;
using OrangeFinance.Domain.Farms.Enums;
using OrangeFinance.Domain.Farms.Models;
using OrangeFinance.Domain.Farms.ValueObjects;
using OrangeFinance.Domain.Harvests.Enums;

namespace OrangeFinance.Domain.Harvests;

public sealed class Harvest : AggregateRoot<HarvestId, Guid>
{
    private Harvest(Guid id, string description, DateTime harvestDate, double quantity, CropType cropType, Guid farmId, TypeUnit typeUnit)
    {
        Id = id;
        Description = description;
        HarvestDate = harvestDate;
        Quantity = quantity;
        CropType = cropType;
        FarmId = farmId;
        TypeUnit = typeUnit;
    }

    public Guid Id { get; init; }
    public string Description { get; init; };
    public DateTime HarvestDate { get; init; }
    public double Quantity { get; init; }
    public CropType CropType { get; init; }
    public TypeUnit TypeUnit { get; init; }
    public Guid FarmId { get; init; }
    public FarmModel? Farm { get; init; }

    public static Harvest Create(string description, DateTime harvestDate, double quantity, int cropType, Guid farmId, int typeUnit)
    {
        return new Harvest(id: Guid.NewGuid(), description: description, harvestDate: harvestDate, quantity: quantity, cropType: (CropType)cropType, farmId: farmId, typeUnit: (TypeUnit)typeUnit);
    }
}
