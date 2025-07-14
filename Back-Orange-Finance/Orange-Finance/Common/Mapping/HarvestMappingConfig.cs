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
            .ConstructUsing(src => Harvest.Create(src.Description, src.HarvestDate, src.Quantity, src.CropType, src.FarmId));


        config.NewConfig<Harvest, HarvestModel>()
            .ConstructUsing(src => new HarvestModel()
            {
                Description = src.Description,
                HarvestDate = src.HarvestDate,
                Quantity = src.Quantity,
                CropType = src.CropType,
                FarmId = src.FarmId,
            })
            .Ignore(dest => dest.Farm);
    }
}
