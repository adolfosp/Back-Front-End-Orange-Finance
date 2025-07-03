using ErrorOr;
using MediatR;

namespace OrangeFinance.Application.Farms.Commands.DeleteFarm;

public record DeleteFarmCommand(Guid Id) : IRequest<ErrorOr<bool>>;
