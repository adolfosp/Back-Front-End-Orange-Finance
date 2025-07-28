using OrangeFinance.Domain.Finances.Enums;
using OrangeFinance.Domain.Harvests.Models;

namespace OrangeFinance.Domain.Finances.Models;

public sealed record class FinanceModel
{
    public FinanceModel(int id, double money, double quantity, TypeTransaction typeTransaction, Guid? harvestId, double total)
    {
        Id = id;
        Money = money;
        Quantity = quantity;
        TypeTransaction = typeTransaction;
        HarvestId = harvestId;
        Total = total;
    }

    public int Id { get; init; }
    public double Money { get; init; }
    public double Quantity { get; init; }
    public double Total { get; init; }
    public TypeTransaction TypeTransaction { get; init; }

    // Relacionamento com a colheita
    public Guid? HarvestId { get; init; } // A chave estrangeira para a colheita
    public HarvestModel? Harvest { get; set; }  // Propriedade de navegação

    public static FinanceModel Create(double quantity, TypeTransaction typeTransaction, Guid? harvestId)
    {
        return new FinanceModel(id: 0, money: 0, quantity: quantity, typeTransaction: typeTransaction, harvestId: harvestId, total: 0);
    }
}
