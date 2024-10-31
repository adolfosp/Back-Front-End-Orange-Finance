using System.IdentityModel.Tokens.Jwt;
using System.Text;

using ErrorOr;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

using OrangeFinance.Application.Common.Extensions;
using OrangeFinance.Application.Security.Dtos;

namespace OrangeFinance.Application.Security;

public sealed class SecurityService(IHttpClientFactory httpClientFactory, IConfiguration section, ILogger<SecurityService> logger)
{

    public async Task<ErrorOr<string>> GetApiTokenAsync(LoginDto dto)
    {
        logger.LogInformation("Getting token for user {Email}", dto.Email);
        return await CreateRemoteTokenAsync(dto);
    }

    private ErrorOr<string> CreateLocalToken(LoginDto dto)
    {
        var handler = new JwtSecurityTokenHandler();

        var privateKey = Encoding.UTF8.GetBytes(section.GetSection("Jwt:Key").Get<string>()!);

        var credentials = new SigningCredentials(
            new SymmetricSecurityKey(privateKey),
            SecurityAlgorithms.HmacSha256);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            SigningCredentials = credentials,
            Expires = DateTime.UtcNow.AddHours(1),
            Subject = {
            }
        };

        var token = handler.CreateToken(tokenDescriptor);
        return handler.WriteToken(token);
    }

    private async Task<ErrorOr<string>> CreateRemoteTokenAsync(LoginDto dto)
    {
        logger.LogInformation("Start {@Dto}", dto);

        try
        {
            var client = httpClientFactory.CreateClient("Back-Authentication");
            logger.LogInformation("Start {@client}", client);

            var content = dto.ToJsonContent();

            var response = await client.PostAsync("/api/sessions", content);

            response.EnsureSuccessStatusCode();

            var token = await response.Content.ReadAsStringAsync();

            return token;
        }
        catch (Exception ex)
        {
            return Error.Failure(code: "User.Token", description: ex.Message);
        }
    }
}
