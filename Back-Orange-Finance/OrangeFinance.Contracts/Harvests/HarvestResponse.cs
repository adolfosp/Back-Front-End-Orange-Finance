namespace OrangeFinance.Contracts.Harvests;

public record HarvestResponse(
    Guid Id,
    Guid FarmId,
    string Description,
    DateTime HarvestDate,
    double Quantity,
    int CropType);


