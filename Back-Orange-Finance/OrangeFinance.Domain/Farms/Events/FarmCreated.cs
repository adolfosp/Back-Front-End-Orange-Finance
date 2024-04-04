using OrangeFinance.Domain.Common.Models;

namespace OrangeFinance.Domain.Farms.Events;

public record FarmCreated(Guid Id, string Name, string Description, double Longitude, double Latitude, string Size, string Type, string Image) : IDomainEvent;
