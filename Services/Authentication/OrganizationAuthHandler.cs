using LuneauAuthentication.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace LuneauAuthentication.Services
{
    public class OrganizationAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public const string SCHEME_NAME = "OrganizationAuthentication";
        private readonly IMongoCollection<Organization> OrganizationCollection;
        public OrganizationAuthHandler(
            IMongoCollection<Organization> organizationCollection,
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock)
            : base(options, logger, encoder, clock)
        {
            OrganizationCollection = organizationCollection;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.TryGetValue("Authorization", out var authorization))
                return AuthenticateResult.Fail("Missing Authorization Header");

            var authHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
            string apiKey = authHeader.ToString();
            Organization organization = await OrganizationCollection.AsQueryable().FirstOrDefaultAsync(e => e.ApiKey == apiKey);

            if (organization == null)
                return AuthenticateResult.Fail($"Can't find organization with for api key {apiKey}");

            var claims = new[] {
                new Claim("OrganizationId", organization.Id.ToString()),
            };
            var claimIdentity = new ClaimsIdentity(claims, "apikey");
            var ticket = new AuthenticationTicket(new ClaimsPrincipal(claimIdentity), SCHEME_NAME);

            return AuthenticateResult.Success(ticket);
        }
    }
}
