using LuneauAuthentication.Models;
using LuneauAuthentication.Outputs;
using LuneauAuthentication.Services;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Threading.Tasks;

namespace LuneauAuthentication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [OrganizationAuthorization]
    public class OrganizationController : ControllerBase
    {
        private readonly JwtService JwtService;
        private readonly IMongoCollection<Organization> OrganizationCollection;
        public OrganizationController(JwtService jwtService, IMongoCollection<Organization> organizationCollection)
        {
            JwtService = jwtService;
            OrganizationCollection = organizationCollection;
        }

        [HttpGet("token")]
        public async Task<TokenOutput> GetToken()
        {
            Guid organizationId = Guid.Parse(User.FindFirst("organizationId").Value);
            Organization organization = await OrganizationCollection.AsQueryable().FirstOrDefaultAsync(o => o.Id == organizationId);
            return new TokenOutput(JwtService.GenerateToken(organization));
        }
    }
}