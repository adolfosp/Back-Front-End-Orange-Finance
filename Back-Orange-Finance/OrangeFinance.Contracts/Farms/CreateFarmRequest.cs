namespace OrangeFinance.Contracts.Farms;

public record CreateFarmRequest(string Name, string Description, double Longitude, double Latitude, string Size, string Type, string Image);

