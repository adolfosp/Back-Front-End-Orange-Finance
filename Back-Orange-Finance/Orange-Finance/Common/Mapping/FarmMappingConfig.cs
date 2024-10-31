using Mapster;

using OrangeFinance.Application.Farms.Commands.CreateFarm;
using OrangeFinance.Contracts.Farms;
using OrangeFinance.Domain.Common.ValueObject;
using OrangeFinance.Domain.Farms;
using OrangeFinance.Domain.Farms.Models;
using OrangeFinance.Domain.Farms.ValueObjects;
using OrangeFinance.GraphQL.Farms.Types;

namespace OrangeFinance.Common.Mapping;

public class FarmMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<CreateFarmRequest, CreateFarmCommand>()
            .Map(dest => dest.Name, src => src.Name)
            .Map(dest => dest.Description, src => src.Description)
            .Map(dest => dest.Location, src => new Coordinate(src.Latitude, src.Longitude))
            .Map(dest => dest.Size, src => src.Size)
            .Map(dest => dest.Type, src => src.Type)
            .Map(dest => dest.Image, src => src.Image)
            .Map(dest => dest.Cnpj, src => new Cnpj(src.Cnpj));

        config.NewConfig<Farm, FarmModel>()
             .ConstructUsing(src => new FarmModel(src.Id.Value,
                                                      src.Name,
                                                      src.Description,
                                                      src.Location.Longitude,
                                                      src.Location.Latitude,
                                                      src.Size,
                                                      src.Type,
                                                      src.Image,
                                                      src.Cnpj.Value));

        config.NewConfig<Farm, FarmResponse>()
            .ConstructUsing(src => new FarmResponse(src.Id.Value,
                                                src.Name,
                                                src.Description,
                                                src.Location.Longitude,
                                                src.Location.Latitude,
                                                src.Size,
                                                src.Type,
                                                src.Image,
                                                src.Cnpj.Value));

        config.NewConfig<Farm, FarmResponseType>()
           .ConstructUsing(src => new FarmResponseType());

        config.NewConfig<FarmModel, FarmResponse>()
            .ConstructUsing(src => new FarmResponse(src.Id,
                                               src.Name,
                                               src.Description,
                                               src.Longitude,
                                               src.Latitude,
                                               src.Size,
                                               src.Type,
                                               src.Image,
                                               src.Cnpj));

    }
}
