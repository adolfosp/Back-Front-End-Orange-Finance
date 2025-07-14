using OrangeFinance.Domain.Common.Models;

namespace OrangeFinance.Domain.Farms.ValueObjects;

/// <summary>
/// Usar GUID como Id prejudica a performance no banco de dados na parte de indexação
/// </summary>
public sealed class HarvestId : AggregateRootId<Guid>
{
    public HarvestId(Guid value)
    {
        Value = value;
    }
    private HarvestId() { }
    public override Guid Value { get; protected set; }

    public static HarvestId Create(Guid value)
    {
        return new(value);
    }

    public static HarvestId CreateUnique()
    {
        return new(Guid.NewGuid());
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

}
