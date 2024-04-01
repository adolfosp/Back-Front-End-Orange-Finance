using ErrorOr;

using MediatR;

namespace OrangeFinance.Application.Farms.Commands.CreateFarm;


public sealed class CreateFarmCommandHandler : IRequestHandler<CreateFarmCommand, ErrorOr<string>>
{

    public CreateFarmCommandHandler()
    {

    }

    public async Task<ErrorOr<string>> Handle(CreateFarmCommand request, CancellationToken cancellationToken)
    {
        return "Farm created successfully!";
    }
}
