﻿using Mapster;

using OrangeFinance.Application.Farms.Commands.CreateFarm;
using OrangeFinance.Contracts.Farms;
using OrangeFinance.Domain.Farms;
using OrangeFinance.Domain.Farms.Models;
using OrangeFinance.Domain.Farms.ValueObjects;

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
            .Map(dest => dest.Image, src => src.Image);

        config.NewConfig<Farm, FarmModel>()
             .ConstructUsing(src => new FarmModel(src.Id.Value,
                                                      src.Name,
                                                      src.Description,
                                                      src.Location.Longitude,
                                                      src.Location.Latitude,
                                                      src.Size,
                                                      src.Type,
                                                      src.Image));

        config.NewConfig<Farm, FarmResponse>()
       .ConstructUsing(src => new FarmResponse(src.Id.Value.ToString(),
                                                src.Name,
                                                src.Description,
                                                src.Location.Longitude,
                                                src.Location.Latitude,
                                                src.Size,
                                                src.Type,
                                                src.Image));

    }
}
