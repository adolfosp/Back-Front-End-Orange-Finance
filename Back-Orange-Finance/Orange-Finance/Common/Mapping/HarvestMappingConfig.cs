using Mapster;
using OrangeFinance.Contracts.Harvests;
using OrangeFinance.Domain.Harvests;
using OrangeFinance.Domain.Harvests.Models;

namespace OrangeFinance.Common.Mapping;

public class HarvestMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<HarvestsDto, Harvest>()
            .ConstructUsing(src => Harvest.Create(src.Description, src.HarvestDate, src.Quantity, src.CropType, src.FarmId, src.TypeUnit));


        config.NewConfig<Harvest, HarvestModel>()
            .ConstructUsing(src => new HarvestModel(
              src.Id,
              src.Description,
              src.HarvestDate,
              src.Quantity,
              src.CropType,
              src.FarmId,
              src.TypeUnit
            )).Ignore(src => src.Farm);

        config.NewConfig<HarvestModel, HarvestResponse>()
            .ConstructUsing(src => new HarvestResponse(
                src.Id,
                src.FarmId,
                src.Description,
                src.HarvestDate,
                src.Quantity,
                (int)src.CropType));
    }
}
