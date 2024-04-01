using OrangeFinance.Domain.Common.Models;

namespace OrangeFinance.Domain.Farms.Events;

public record FarmCreated(Farm farm) : IDomainEvent;