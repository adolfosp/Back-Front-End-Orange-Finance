using OrangeFinance.Domain.Farms.Enums;

namespace OrangeFinance.Domain.Farms.Models;

public record class HarvestModel
{
    public Guid Id { get; init; }
    public string Description { get; init; } = default!;
    public DateTime HarvestDate { get; init; }
    public double Quantity { get; init; }
    public CropType CropType { get; init; }

    // Relacionamento com Farm
    public Guid FarmId { get; init; }
    public FarmModel? Farm { get; init; }  // Propriedade de navegação
}
