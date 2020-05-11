using LuneauAuthentication.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace LuneauAuthentication.Services
{
    public class OrganizationAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly IMongoCollection<Organization> OrganizationCollection;
        private readonly AdminCredentialInfos AdminCredentials;
        public OrganizationAuthHandler(
            IMongoCollection<Organization> organizationCollection,
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            IOptions<AppSettings> appSettings)
            : base(options, logger, encoder, clock)
        {
            AdminCredentials = appSettings.Value.AdminCredentials;
            OrganizationCollection = organizationCollection;
        }

        private async Task<AuthenticateResult> HandleAuthenticate()
        {
            if (!Request.Headers.TryGetValue("Authorization", out var authorization))
                return AuthenticateResult.Fail("Missing Authorization Header");

            var authHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
            OrganizationCollection.AsQueryable().FirstAsync(e => e.ApiKey == "a");


            return AuthenticateResult.Success(ticket);
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
            => Task.FromResult(HandleAuthenticate());
    }
}
