using ErrorOr;

using MediatR;

using OrangeFinance.Application.Common.Interfaces.Persistence.Farms;
using OrangeFinance.Domain.Farms.Models;

namespace OrangeFinance.Application.Farms.Queries.Farms;

public sealed class FarmQueryHandler : IRequestHandler<FarmQuery, ErrorOr<IEnumerable<FarmModel>?>>
{
    private readonly IReadFarmRepository _readFarmRepository;
    public FarmQueryHandler(IReadFarmRepository readFarmRepository)
    {
        _readFarmRepository = readFarmRepository;
    }

    public async Task<ErrorOr<IEnumerable<FarmModel>>> Handle(FarmQuery request, CancellationToken cancellationToken)
    {
        var farms = await _readFarmRepository.GetAllAsync(pagination: request.Pagination);

        return farms.ToList();
    }
}
