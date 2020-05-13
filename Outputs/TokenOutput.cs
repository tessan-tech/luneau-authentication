using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LuneauAuthentication.Outputs
{
    public class TokenOutput
    {
        public TokenOutput(string accessToken)
        {
            AccessToken = accessToken;
        }

        public string AccessToken { get; set; }
    }
}
