using LuneauAuthentication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LuneauAuthentication.Outputs
{
    public class OrganizationOutput
    {
        public OrganizationOutput(Organization organization)
        {
            Id = organization.Id;
            ApiKey = organization.ApiKey;
            Name = organization.Name;
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string ApiKey { get; set; }

        public static OrganizationOutput Create(Organization organization) => new OrganizationOutput(organization);
    }
}
