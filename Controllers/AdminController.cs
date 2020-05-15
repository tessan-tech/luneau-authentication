using LuneauAuthentication.Inputs;
using LuneauAuthentication.Models;
using LuneauAuthentication.Outputs;
using LuneauAuthentication.Services;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LuneauAuthentication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AdminAuthorization]
    public class AdminController : ControllerBase
    {
        private readonly IMongoCollection<Organization> OrganizationCollection;
        public AdminController(IMongoCollection<Organization> organizationCollection)
        {
            OrganizationCollection = organizationCollection;
        }

        [HttpPost("organization")]
        public async Task<ActionResult<Organization>> CreateOrganization(OrganizationInput input)
        {
            if (await OrganizationCollection.AsQueryable().AnyAsync(o => o.Name == input.Name))
                return BadRequest($"Name {input.Name} already taken");
            var organization = new Organization(input.Name);
            await OrganizationCollection.InsertOneAsync(organization);
            return new OkObjectResult(new OrganizationOutput(organization));
        }

        [HttpGet("organizations")]
        public IEnumerable<OrganizationOutput> ListOrganizations([Required]int from, [Required]int limit)
        {
            return OrganizationCollection
                .AsQueryable()
                .Skip(from)
                .Take(limit)
                .Select(OrganizationOutput.Create);
        }
    }
}