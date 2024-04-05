using ErrorOr;

using MediatR;

using OrangeFinance.Domain.Farms.Models;

namespace OrangeFinance.Application.Farms.Queries.Farms;

public record FarmQuery() : IRequest<ErrorOr<IEnumerable<FarmModel>>>;