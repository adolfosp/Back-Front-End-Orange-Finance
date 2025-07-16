using OrangeFinance.Domain.Farms.Enums;
using OrangeFinance.Domain.Farms.Models;
using OrangeFinance.Domain.Finances.Models;
using OrangeFinance.Domain.Harvests.Enums;

namespace OrangeFinance.Domain.Harvests.Models;

public sealed record class HarvestModel
{
    public Guid Id { get; init; }
    public string Description { get; init; }
    public DateTime HarvestDate { get; init; }
    public double Quantity { get; init; }
    public CropType CropType { get; init; }
    public TypeUnit TypeUnit { get; init; }

    // Relacionamento com a Fazenda
    public Guid FarmId { get; init; }
    public FarmModel? Farm { get; set; }  // Propriedade de navegação

    // Relacionamento com FinanceModel
    public ICollection<FinanceModel>? Finances { get; set; }  // Coleção de transações financeiras associadas à colheita


    public HarvestModel(Guid id, string description, DateTime harvestDate, double quantity, CropType cropType, Guid farmId, TypeUnit typeUnit)
    {
        Id = id;
        Description = description;
        HarvestDate = DateTime.SpecifyKind(harvestDate.ToUniversalTime(), DateTimeKind.Utc);
        Quantity = quantity;
        CropType = cropType;
        FarmId = farmId;
        TypeUnit = typeUnit;
    }
}
