namespace OrangeFinance.Domain.Common.ValueObject;

public record Cnpj
{
    public string Value { get; }

    public Cnpj(string value)
    {
        if (!IsValid(value))
        {
            throw new ArgumentException("Invalid CNPJ");
        }

        Value = value;
    }

    public static bool IsValid(string cnpj)
    {
        if (string.IsNullOrWhiteSpace(cnpj))
        {
            return false;
        }

        cnpj = cnpj.Replace(".", "").Replace("-", "").Replace("/", "");

        if (cnpj.Length != 14)
        {
            return false;
        }

        var numbers = cnpj.Select(x => int.Parse(x.ToString())).ToArray();

        var sum = 0;
        var multiplier = 2;

        for (var i = 11; i >= 0; i--)
        {
            sum += numbers[i] * multiplier;

            multiplier = multiplier == 9 ? 2 : multiplier + 1;
        }

        var mod = sum % 11;

        var digit = mod < 2 ? 0 : 11 - mod;

        if (numbers[12] != digit)
        {
            return false;
        }

        sum = 0;
        multiplier = 2;

        for (var i = 12; i >= 0; i--)
        {
            sum += numbers[i] * multiplier;

            multiplier = multiplier == 9 ? 2 : multiplier + 1;
        }

        mod = sum % 11;

        digit = mod < 2 ? 0 : 11 - mod;

        return numbers[13] == digit;
    }
}
