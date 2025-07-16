using OrangeFinance.Domain.Finances.Enums;
using OrangeFinance.Domain.Harvests.Models;

namespace OrangeFinance.Domain.Finances.Models;

public sealed record class FinanceModel
{
    public FinanceModel(int id, double money, int quantity, TypeTransaction typeTransaction, Guid? harvestId)
    {
        Id = id;
        Money = money;
        Quantity = quantity;
        TypeTransaction = typeTransaction;
        HarvestId = harvestId;
    }

    public int Id { get; init; }
    public double Money { get; init; }
    public int Quantity { get; init; }
    public TypeTransaction TypeTransaction { get; init; }

    // Relacionamento com a colheita
    public Guid? HarvestId { get; init; } // A chave estrangeira para a colheita
    public HarvestModel? Harvest { get; set; }  // Propriedade de navegação
}
