namespace OrangeFinance.Application.Farms.Commands.CreateFarm;

public record CreateFarmCommand()
{
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public Coordinate Location { get; init; } = new();
    public string Size { get; init; } = string.Empty;
    public string Type { get; init; } = string.Empty;
    public string Image { get; init; } = string.Empty;
}


public class Coordinate
{
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}
