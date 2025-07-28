using ErrorOr;
using MapsterMapper;
using OrangeFinance.Application.Common.Interfaces.Persistence;
using OrangeFinance.Application.Common.Interfaces.Persistence.Farms;
using OrangeFinance.Application.Common.Interfaces.Persistence.Finances;
using OrangeFinance.Application.Common.Interfaces.Persistence.Harvests;
using OrangeFinance.Domain.Finances.Enums;
using OrangeFinance.Domain.Finances.Models;
using OrangeFinance.Domain.Harvests;
using OrangeFinance.Domain.Harvests.Models;

namespace OrangeFinance.Application.Harvests;

public sealed class HarvestsAppService(IWriteHarvestRepository harvestRepository,
                                       IReadFarmRepository readFarmRepository,
                                       IMapper mapper,
                                       IUnitOfWork unitOfWork,
                                       IWriteFinanceRepository writeFinanceRepository)
{
    private readonly IWriteHarvestRepository _harvestRepository = harvestRepository;
    private readonly IReadFarmRepository _farmReadRepository = readFarmRepository;
    private readonly IMapper _mapper = mapper;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IWriteFinanceRepository _financeRepository = writeFinanceRepository;

    public async Task<ErrorOr<HarvestModel>> CreateHarvestAsync(Harvest model)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(model);

            var harvestModel = _mapper.Map<HarvestModel>(model);

            var farm = await _farmReadRepository.GetByIdAsync(model.FarmId);

            ArgumentNullException.ThrowIfNull(farm);

            await _harvestRepository.AddAsync(harvestModel, CancellationToken.None);

            var financeModel = FinanceModel.Create(quantity: model.Quantity,
                                                   typeTransaction: TypeTransaction.Income,
                                                   harvestId: harvestModel.Id);

            await _financeRepository.AddAsync(financeModel, CancellationToken.None);

            var result = await _unitOfWork.SaveChangesAsync(CancellationToken.None);

            if (result <= 0)
            {
                return Error.Failure("Failed to save the harvest.");
            }

            return harvestModel;
        }
        catch (Exception ex)
        {
            return Error.Failure("An error occurred while creating the harvest.", ex.Message);
        }

    }
}
