using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Restaurants.Api.IntegrationTests.Services;

/// <summary>
/// Fake AuthenticationHandler
/// This bypasses Entra ID and enables testing protected endpoints as if the user were authenticated.
///   - replace the real authentication scheme with a test scheme
///   - injects fake claims (e.g. user ID, roles) to simulate authenticated users.
/// </summary>
public sealed class FakeAuthHandler(
    IOptionsMonitor<AuthenticationSchemeOptions> options,
    ILoggerFactory logger,
    UrlEncoder encoder) : AuthenticationHandler<AuthenticationSchemeOptions>(options, logger, encoder)
{
    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var claims = new[] {
            new Claim(ClaimTypes.NameIdentifier, "admin-user-id"),
            new Claim(ClaimTypes.Name, "Admin User"),
            new Claim(ClaimTypes.Role, "Admin")
        };

        var identity = new ClaimsIdentity(claims, "Test");
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, "FakeAuth");

        return Task.FromResult(AuthenticateResult.Success(ticket));
    }
}