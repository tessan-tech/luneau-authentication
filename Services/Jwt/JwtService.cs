using LuneauAuthentication.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace LuneauAuthentication.Services
{
    public class JwtService
    {
        private readonly JwtInfos JwtInfos;
        private readonly RSA Rsa;

        public JwtService(IOptions<AppSettings> appSettings)
        {
            JwtInfos = appSettings.Value.Jwt;
            Rsa = RSA.Create();
            Rsa.ImportRSAPrivateKey(Convert.FromBase64String(JwtInfos.PrivateKey), out int trolo);
        }

        public string GenerateToken(Organization organization)
        {
            var key = new RsaSecurityKey(Rsa);
            var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.RsaSha256);

            var jwtDate = DateTime.UtcNow;

            var claims = new Claim[]
            {
                new Claim("organizationId", organization.Id.ToString()),
                new Claim("organizationName", organization.Name)
            };

            var jwt = new JwtSecurityToken(
                audience: "organization",
                issuer: "luneau-auth-server",
                claims: claims,
                notBefore: jwtDate,
                expires: jwtDate.AddMinutes(JwtInfos.ExpirationMinutes),
                signingCredentials: signingCredentials
            );

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }
    }
}
