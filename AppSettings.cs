namespace LuneauAuthentication
{
    public class JwtInfos
    {
        public string PrivateKey { get; set; }
        public string PublicKey { get; set; }
        public int ExpirationMinutes { get; set; }
    }

    public class AdminCredentialInfos
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class MongoInfos
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }

    public class AppSettings
    {
        public JwtInfos Jwt { get; set; }
        public AdminCredentialInfos AdminCredentials { get; set; }
        public MongoInfos Mongo { get; set; }
    }
}
