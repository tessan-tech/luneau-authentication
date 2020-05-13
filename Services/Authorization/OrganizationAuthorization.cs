using Microsoft.AspNetCore.Authorization;

namespace LuneauAuthentication.Services
{
    public class OrganizationAuthorization: AuthorizeAttribute
    {
        public OrganizationAuthorization() : base()
        {
            AuthenticationSchemes = "OrganizationAuthentication";
        }
    }
}
