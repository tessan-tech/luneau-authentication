using Microsoft.AspNetCore.Authorization;

namespace LuneauAuthentication.Services
{
    public class AdminAuthorization: AuthorizeAttribute
    {
        public AdminAuthorization() : base()
        {
            AuthenticationSchemes = "AdminAuthentication";
        }
    }
}
