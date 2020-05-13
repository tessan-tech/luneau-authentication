using LuneauAuthentication.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
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

        public JwtService(IOptions<AppSettings> appSettings)
        {
            JwtInfos = appSettings.Value.Jwt;
        }

        public string GenerateToken(Organization organization)
        {
            using RSA rsa = RSA.Create();
            rsa.ImportRSAPrivateKey(
                source: Convert.FromBase64String(JwtInfos.PrivateKey),
                bytesRead: out int _);

            var signingCredentials = new SigningCredentials(
                key: new RsaSecurityKey(rsa),
                algorithm: SecurityAlgorithms.RsaSha256
            );

            DateTime jwtDate = DateTime.UtcNow;

            Claim[] claims = new Claim[]
            {
                new Claim("organizationId", organization.Id.ToString()),
                new Claim("organizationName", organization.Name)
            };

            var jwt = new JwtSecurityToken(
                audience: "organization",
                issuer: "luneau auth server",
                claims: claims,
                notBefore: jwtDate,
                expires: jwtDate.AddMinutes(JwtInfos.ExpirationMinutes),
                signingCredentials: signingCredentials
            );

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }
    }
}
