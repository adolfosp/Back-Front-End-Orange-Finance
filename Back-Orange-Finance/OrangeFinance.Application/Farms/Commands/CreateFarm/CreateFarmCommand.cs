using ErrorOr;

using MediatR;

using OrangeFinance.Domain.Farms.ValueObjects;

namespace OrangeFinance.Application.Farms.Commands.CreateFarm;

public record CreateFarmCommand(string Name, string Description, Coordinate Location, string Size, string Type, string Image) : IRequest<ErrorOr<string>>;
