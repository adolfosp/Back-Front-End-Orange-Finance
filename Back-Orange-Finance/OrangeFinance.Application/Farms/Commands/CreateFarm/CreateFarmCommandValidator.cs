using FluentValidation;

namespace OrangeFinance.Application.Farms.Commands.CreateFarm;

public sealed class CreateFarmCommandValidator : AbstractValidator<CreateFarmCommand>
{
    public CreateFarmCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.Description).NotEmpty();
        RuleFor(x => x.Location).NotNull();
        RuleFor(x => x.Size).NotEmpty();
        RuleFor(x => x.Type).NotEmpty();
        RuleFor(x => x.Image).NotEmpty();

    }
}
