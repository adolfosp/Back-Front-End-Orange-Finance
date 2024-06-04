using ErrorOr;

using MediatR;

using OrangeFinance.Domain.Common.Models;
using OrangeFinance.Domain.Farms.Models;

namespace OrangeFinance.Application.Farms.Queries.Farms;

public record FarmQuery(Pagination Pagination) : IRequest<ErrorOr<IEnumerable<FarmModel>>>;