using OrangeFinance.Domain.Harvests.Models;

namespace OrangeFinance.Domain.Farms.Models;

public record class FarmModel
{
    public FarmModel(Guid id, string name, string description, double longitude, double latitude, string size, string type, string? image, string cnpj)
    {
        Id = id;
        Name = name;
        Description = description;
        Longitude = longitude;
        Latitude = latitude;
        Size = size;
        Type = type;
        Image = image;
        Cnpj = cnpj;
    }

    public Guid Id { get; init; }
    public string Name { get; init; } = default!;
    public string Description { get; init; } = default!;
    public double Longitude { get; init; }
    public double Latitude { get; init; }
    public string Size { get; init; } = default!;
    public string Type { get; init; } = default!;
    public string? Image { get; init; }
    public string Cnpj { get; init; } = default!;

    // Propriedade de navegação
    public ICollection<HarvestModel> Harvests { get; init; } = new List<HarvestModel>();
}
