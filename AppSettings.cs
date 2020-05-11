using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LuneauAuthentication
{
    public class JwtInfos
    {
        public string PrivateKey { get; set; }
        public string PublicKey { get; set; }
        public int ExpirationTime { get; set; }
    }

    public class AdminCredentialInfos
    {
        public string Username;
        public string Password;
    }

    public class MongoInfos
    {
        public string ConnectionString;
        public string DatabaseName;
    }

    public class AppSettings
    {
        public JwtInfos Jwt { get; set; }
        public AdminCredentialInfos AdminCredentials { get; set; }
        public MongoInfos Mongo { get; set; }
    }
}
