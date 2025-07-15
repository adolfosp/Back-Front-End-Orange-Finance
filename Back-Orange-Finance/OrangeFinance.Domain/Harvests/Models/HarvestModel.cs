using OrangeFinance.Domain.Farms.Enums;
using OrangeFinance.Domain.Farms.Models;

namespace OrangeFinance.Domain.Harvests.Models;

public record class HarvestModel
{
    public Guid Id { get; init; }
    public string Description { get; init; } = default!;
    public DateTime HarvestDate { get; init; }
    public double Quantity { get; init; }
    public CropType CropType { get; init; }

    // Relacionamento com Farm
    public Guid FarmId { get; init; }
    public FarmModel? Farm { get; set; }  // Propriedade de navegação

    public HarvestModel(Guid id, string description, DateTime harvestDate, double quantity, CropType cropType, Guid farmId)
    {
        Id = id;
        Description = description;
        HarvestDate = DateTime.SpecifyKind(harvestDate.ToUniversalTime(), DateTimeKind.Utc);
        Quantity = quantity;
        CropType = cropType;
        FarmId = farmId;
    }
}
