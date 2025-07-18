﻿using OrangeFinance.Domain.Common.Models;

namespace OrangeFinance.Domain.Farms.ValueObjects;

/// <summary>
/// Usar GUID como Id prejudica a performance no banco de dados na parte de indexação
/// </summary>
public sealed class FarmId : AggregateRootId<Guid>
{
    public FarmId(Guid value)
    {
        Value = value;
    }
    private FarmId() { }
    public override Guid Value { get; protected set; }

    public static FarmId Create(Guid value)
    {
        return new(value);
    }

    public static FarmId CreateUnique()
    {
        return new(Guid.NewGuid());
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

}
