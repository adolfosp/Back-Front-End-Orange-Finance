using System.ComponentModel.DataAnnotations;

namespace OrangeFinance.Contracts.Harvests;


public record HarvestsDto
{
    [Required(ErrorMessage = "Description is required.")]
    public required string Description { get; init; }

    [Required(ErrorMessage = "HarvestDate is required.")]
    public required DateTime HarvestDate { get; init; }

    [Range(0, double.MaxValue, ErrorMessage = "Quantity must be a positive value.")]
    public required double Quantity { get; init; }

    [Range(1, 9, ErrorMessage = "CropType must be a valid integer greater than 0.")]
    public required int CropType { get; init; }

    [Required(ErrorMessage = "FarmId is required.")]
    public required Guid FarmId { get; init; }
}