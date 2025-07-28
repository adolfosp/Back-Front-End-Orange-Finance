using OrangeFinance.Domain.Finances.Enums;

namespace OrangeFinance.Domain.Finances;

public sealed record Finance
{
    public Finance(int id, double money, int quantity, TypeTransaction typeTransaction, double total)
    {
        Id = id;
        Money = money;
        Quantity = quantity;
        TypeTransaction = typeTransaction;
        Total = total;
    }
    public int Id { get; init; }
    public double Money { get; init; }
    public int Quantity { get; init; }
    public double Total { get; init; }
    public TypeTransaction TypeTransaction { get; init; }
}
