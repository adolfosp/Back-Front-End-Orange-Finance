namespace OrangeFinance.Domain.Farms.Models;

public record class FarmModel(Guid Id, string Name, string Description, double Longitude, double Latitude, string Size, string Type, string? Image, string Cnpj);