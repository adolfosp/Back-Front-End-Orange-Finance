using ErrorOr;

using MediatR;

using OrangeFinance.Domain.Common.ValueObject;
using OrangeFinance.Domain.Farms;
using OrangeFinance.Domain.Farms.ValueObjects;

namespace OrangeFinance.Application.Farms.Commands.CreateFarm;

public record CreateFarmCommand(string Name, string Description, Coordinate Location, string Size, string Type, string Image, Cnpj Cnpj) : IRequest<ErrorOr<Farm>>;
