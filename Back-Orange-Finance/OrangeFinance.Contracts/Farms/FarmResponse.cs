namespace OrangeFinance.Contracts.Farms;

public record FarmResponse(Guid Id, string Name, string Description, double Longitude, double Latitude, string Size, string Type, string Image, string Cnpj);