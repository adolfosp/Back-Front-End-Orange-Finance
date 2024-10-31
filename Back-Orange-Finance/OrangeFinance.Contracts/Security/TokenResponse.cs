using System.Text.Json.Serialization;

namespace OrangeFinance.Contracts.Security;

public sealed class TokenResponse
{
    [JsonPropertyName("accessToken")]
    public string AccessToken { get; private set; }

    [JsonPropertyName("refreshToken")]
    public string RefreshToken { get; private set; }
    public TokenResponse(string accessToken, string refreshToken)
    {
        AccessToken = accessToken;
        RefreshToken = refreshToken;
    }
};

