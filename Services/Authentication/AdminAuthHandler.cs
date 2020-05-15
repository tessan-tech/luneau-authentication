using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace LuneauAuthentication.Services
{
    public class AdminAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public const string SCHEME_NAME = "AdminAuthentication";
        private readonly AdminCredentialInfos AdminCredentials;
        public AdminAuthHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            IOptions<AppSettings> appSettings)
            : base(options, logger, encoder, clock)
        {
            AdminCredentials = appSettings.Value.AdminCredentials;
        }

        private AuthenticateResult HandleAuthenticate()
        {
            if (!Request.Headers.TryGetValue("Authorization", out var authorization))
                return AuthenticateResult.Fail("Missing Authorization Header");

            var authHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
            if (authHeader.Scheme != "Basic")
                return AuthenticateResult.Fail("Invalid scheme, must be Basic");
            if (string.IsNullOrEmpty(authHeader.Parameter))
                return AuthenticateResult.Fail("Invalid auth header for basic auth");
            var credentialBytes = Convert.FromBase64String(authHeader.Parameter);
            var credentials = Encoding.UTF8.GetString(credentialBytes).Split(':');
            if (credentials.Length != 2)
                return AuthenticateResult.Fail("Invalid auth header for basic auth");
            var username = credentials[0];
            var password = credentials[1];
            if (username != AdminCredentials.Username || password != AdminCredentials.Password)
                return AuthenticateResult.Fail("Invalid username or password");

            var claims = new Claim[0];
            var claimIdentity = new ClaimsIdentity(claims, "basic");
            var ticket = new AuthenticationTicket(new ClaimsPrincipal(claimIdentity), SCHEME_NAME);

            return AuthenticateResult.Success(ticket);
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
            => Task.FromResult(HandleAuthenticate());
    }
}
