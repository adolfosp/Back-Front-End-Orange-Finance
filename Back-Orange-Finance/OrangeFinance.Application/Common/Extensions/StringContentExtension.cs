using System.Text;
using System.Text.Json;

namespace OrangeFinance.Application.Common.Extensions;

internal static class StringContentExtension
{
    public static StringContent ToJsonContent<T>(this T obj)
    {
        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        var jsonContent = JsonSerializer.Serialize(obj, options);
        return new StringContent(jsonContent, Encoding.UTF8, "application/json");
    }
}
