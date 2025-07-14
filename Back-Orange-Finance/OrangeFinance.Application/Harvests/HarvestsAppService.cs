using MapsterMapper;
using OrangeFinance.Application.Common.Interfaces.Persistence.Harvests;
using OrangeFinance.Domain.Harvests;
using OrangeFinance.Domain.Harvests.Models;

namespace OrangeFinance.Application.Harvests;

public sealed class HarvestsAppService
{
    private readonly IWriteHarvestRepository _harvestRepository;
    private readonly IMapper _mapper;

    public HarvestsAppService(IWriteHarvestRepository harvestRepository, IMapper mapper)
    {
        _harvestRepository = harvestRepository;
        _mapper = mapper;
    }

    public async Task CreateHarvestAsync(Harvest model)
    {
        ArgumentNullException.ThrowIfNull(model);

        var farmModel = _mapper.Map<HarvestModel>(model);

        await _harvestRepository.AddAsync(farmModel, CancellationToken.None);

        await Task.CompletedTask;
    }
}
