using OrangeFinance.Domain.Finances.Enums;

namespace OrangeFinance.Domain.Finances;

public sealed class Finance
{
    public int Id { get; init; }
    public double Money { get; init; }
    public int Quantity { get; init; }
    public TypeTransaction TypeTransaction { get; init; }

}
