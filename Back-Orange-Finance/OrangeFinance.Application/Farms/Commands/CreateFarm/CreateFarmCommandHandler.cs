﻿using ErrorOr;

using MapsterMapper;

using MediatR;
using OrangeFinance.Application.Common.Interfaces.Persistence;
using OrangeFinance.Application.Common.Interfaces.Persistence.Farms;
using OrangeFinance.Domain.Farms;
using OrangeFinance.Domain.Farms.Models;

namespace OrangeFinance.Application.Farms.Commands.CreateFarm;

/// <summary>
/// Separado por escrita e leitura e isso feito pela forma das interfaces do repository
/// </summary>
public sealed class CreateFarmCommandHandler : IRequestHandler<CreateFarmCommand, ErrorOr<Farm>>
{
    private readonly IWriteFarmRepository _writeFarmRepository;
    private readonly IMapper _mapper;
    private readonly IPublisher _publish;
    private readonly IUnitOfWork _unitOfWork;

    public CreateFarmCommandHandler(IWriteFarmRepository writeFarmRepository, IMapper mapper, IPublisher publish, IUnitOfWork unitOfWork)
    {
        _writeFarmRepository = writeFarmRepository;
        _mapper = mapper;
        _publish = publish;
        _unitOfWork = unitOfWork;
    }

    public async Task<ErrorOr<Farm>> Handle(CreateFarmCommand request, CancellationToken cancellationToken)
    {
        var farm = Farm.Create(name: request.Name,
                                    description: request.Description,
                                    longitude: request.Location.Longitude,
                                    latitude: request.Location.Latitude,
                                    size: request.Size,
                                    type: request.Type,
                                    cnpj: request.Cnpj);

        FarmModel? farmModel = _mapper.Map<FarmModel>(farm);

        await _writeFarmRepository.AddAsync(farmModel, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        await _publish.Publish(farm.DomainEvents[0]);

        return farm;

    }
}
