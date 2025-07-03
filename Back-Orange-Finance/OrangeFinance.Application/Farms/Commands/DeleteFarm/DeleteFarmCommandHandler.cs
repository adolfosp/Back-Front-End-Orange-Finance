using ErrorOr;

using MediatR;
using Microsoft.Extensions.Logging;
using OrangeFinance.Application.Common.Interfaces.Persistence.Farms;

namespace OrangeFinance.Application.Farms.Commands.DeleteFarm;

public sealed class DeleteFarmCommandHandler : IRequestHandler<DeleteFarmCommand, ErrorOr<bool>>
{
    private readonly IWriteFarmRepository _writeFarmRepository;
    private readonly ILogger<DeleteFarmCommandHandler> _logger;

    public DeleteFarmCommandHandler(ILogger<DeleteFarmCommandHandler> logger, IWriteFarmRepository writeFarmRepository)
    {
        _writeFarmRepository = writeFarmRepository;
        _logger = logger;
    }

    public async Task<ErrorOr<bool>> Handle(DeleteFarmCommand request, CancellationToken cancellationToken)
    {
        if (request.Id == Guid.Empty)
            return Error.Validation("Invalid farm ID");


#warning criar evento de deleção

        await _writeFarmRepository.DeleteAsync(request.Id, cancellationToken);
        _logger.LogInformation("Farm ID delete: {Id}", request.Id);
        return true;

    }
}
