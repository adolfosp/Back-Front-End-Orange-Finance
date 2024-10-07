using System.Net.Mime;
using System.Text;
using System.Text.Json;

namespace OrangeFinance.Application.Common.Extensions;

internal static class StringContentExtension
{
    public static StringContent ToJsonContent<T>(this T obj)
    {
        return new StringContent(JsonSerializer.Serialize(obj),
            Encoding.UTF8,
            MediaTypeNames.Application.Json);
    }
}
