using ErrorOr;
using MapsterMapper;
using OrangeFinance.Application.Common.Interfaces.Persistence.Farms;
using OrangeFinance.Application.Common.Interfaces.Persistence.Harvests;
using OrangeFinance.Domain.Harvests;
using OrangeFinance.Domain.Harvests.Models;

namespace OrangeFinance.Application.Harvests;

public sealed class HarvestsAppService(IWriteHarvestRepository harvestRepository, IReadFarmRepository readFarmRepository, IMapper mapper)
{
    private readonly IWriteHarvestRepository _harvestRepository = harvestRepository;
    private readonly IReadFarmRepository _farmReadRepository = readFarmRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<ErrorOr<HarvestModel>> CreateHarvestAsync(Harvest model)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(model);

            var harvestModel = _mapper.Map<HarvestModel>(model);

            var farm = await _farmReadRepository.GetByIdAsync(model.FarmId);

            ArgumentNullException.ThrowIfNull(farm);

            await _harvestRepository.AddAsync(harvestModel, CancellationToken.None);

            return harvestModel;
        }
        catch (Exception ex)
        {
            return Error.Failure("An error occurred while creating the harvest.", ex.Message);
        }

    }
}
